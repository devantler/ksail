using System.Diagnostics;
using Devantler.KubernetesProvisioner.Resources.Native;
using k8s;
using k8s.Models;
using KSail.Commands.Check.Extensions;
using KSail.Models;
using KSail.Utils;

namespace KSail.Commands.Check.Handlers;

class KSailCheckCommandHandler
{
  readonly KSailCluster _config;
  readonly HashSet<string> _successfulKustomizations = [];
  string _lastPrintedMessage = "";

  internal KSailCheckCommandHandler(KSailCluster config) => _config = config;

  internal async Task<bool> HandleAsync(CancellationToken cancellationToken = default)
  {
    var stopWatch = Stopwatch.StartNew();
    var stopWatchTotal = Stopwatch.StartNew();
    using var resourceProvisioner = new KubernetesResourceProvisioner(_config.Spec.Context);
    var responseTask = resourceProvisioner.ListKustomizationsWithHttpMessagesAsync();
    var kustomizations = await resourceProvisioner.ListNamespacedCustomObjectAsync<V1CustomResourceDefinitionList>(
      "kustomize.toolkit.fluxcd.io",
      "v1",
      "flux-system",
      "kustomizations",
      cancellationToken: cancellationToken
    ).ConfigureAwait(false);
    int kustomizationsCount = kustomizations.Items.Count;
    await foreach (
      var (watchEventType, kustomization) in
        responseTask.WatchAsync<V1CustomResourceDefinition, object>(cancellationToken: cancellationToken)
    )
    {
      string kustomizationName = kustomization.Metadata.Name;
      var statusConditions = kustomization.Status.Conditions;
      if (statusConditions is null)
      {
        continue;
      }

      if (_successfulKustomizations.Count == kustomizationsCount)
      {
        HandleSuccesfulKustomizations(stopWatchTotal);
        return true;
      }
      else if (_successfulKustomizations.Contains(kustomizationName))
      {
        continue;
      }
      else if (stopWatch.Elapsed.TotalSeconds >= TimeSpanHelper.ParseDuration(_config.Spec.Timeout).Seconds)
      {
        Console.WriteLine(
          $"✗ Kustomization '{kustomizationName}' did not become ready within the specified time limit of" +
          $"{_config.Spec.Timeout} seconds."
        );
        foreach (var statusCondition in statusConditions)
        {
          string? message = statusCondition.Message;
          Console.WriteLine(message);
          Console.WriteLine();
        }
        return false;
      }


      if (HasDependencies(statusConditions))
      {
        continue;
      }
      bool isReady = true;
      foreach (var statusCondition in statusConditions)
      {
        switch (statusCondition.Type)
        {
          case "Stalled":
          case "Failed" when IsCritical(statusCondition):
            HandleFailedStatus(statusCondition, kustomizationName);
            return false;
          case "Ready":
          case "Healthy":
            break;
          default:
            isReady = false;
            break;
        }
      }
      if (isReady)
      {
        HandleReadyStatus(kustomizationName, stopWatch);
        if (_successfulKustomizations.Count == kustomizationsCount)
        {
          HandleSuccesfulKustomizations(stopWatchTotal);
          return true;
        }
      }
      else
      {
        HandleOtherStatus(kustomizationName, stopWatch);
      }
    }

    stopWatch.Stop();
    stopWatchTotal.Stop();
    return true;
  }

  static bool IsCritical(V1CustomResourceDefinitionCondition statusCondition) =>
    statusCondition.Status.Equals("False", StringComparison.Ordinal) &&
    !statusCondition.Reason.Equals("HealthCheckFailed", StringComparison.Ordinal);

  static bool HasDependencies(IList<V1CustomResourceDefinitionCondition> statusConditions) =>
    statusConditions.FirstOrDefault()?.Reason.Equals("DependencyNotReady", StringComparison.Ordinal) ?? false;

  static void HandleSuccesfulKustomizations(Stopwatch stopWatchTotal)
  {
    var totalTimeElapsed = stopWatchTotal.Elapsed;
    int minutes = totalTimeElapsed.Minutes;
    int seconds = totalTimeElapsed.Seconds;
    Console.WriteLine($"✔ All kustomizations are ready! ({minutes}m {seconds}s)");
  }

  void HandleOtherStatus(string kustomizationName, Stopwatch stopWatch)
  {
    var timeElapsed = stopWatch.Elapsed;
    int minutes = timeElapsed.Minutes;
    int seconds = timeElapsed.Seconds;
    string message = $"◎ Waiting for kustomization '{kustomizationName}' to become ready ({minutes}m {seconds}s)";
    if (message != _lastPrintedMessage)
    {
      Console.WriteLine(message);
      _lastPrintedMessage = message;
    }
  }

  void HandleReadyStatus(string kustomizationName, Stopwatch stopWatch)
  {
    var timeElapsed = stopWatch.Elapsed;
    Console.WriteLine(
      $"✔ Kustomization '{kustomizationName}' is ready! ({timeElapsed.Minutes}m {timeElapsed.Seconds}s)"
    );
    _ = _successfulKustomizations.Add(kustomizationName);
    stopWatch.Restart();
  }

  static void HandleFailedStatus(V1CustomResourceDefinitionCondition statusCondition, string kustomizationName)
  {
    string? message = statusCondition.Message;
    Console.WriteLine($"✗ Kustomization '{kustomizationName}' failed with message: {message}");
  }
}
