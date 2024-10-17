using System.Diagnostics;
using Devantler.KubernetesGenerator.KSail.Models;
using Devantler.KubernetesProvisioner.Resources.Native;
using k8s;
using k8s.Models;
using KSail.Commands.Check.Extensions;

namespace KSail.Commands.Check.Handlers;

class KSailCheckCommandHandler : IDisposable
{
  readonly KSailCluster _config;
  readonly KubernetesResourceProvisioner _resourceProvisioner;
  readonly HashSet<string> _kustomizations = [];
  readonly HashSet<string> _successfulKustomizations = [];
  readonly Stopwatch _stopwatch = Stopwatch.StartNew();
  readonly Stopwatch _stopwatchTotal = Stopwatch.StartNew();
  string _lastPrintedMessage = "";

  internal KSailCheckCommandHandler(KSailCluster config)
  {
    _config = config;
    _resourceProvisioner = new KubernetesResourceProvisioner(_config.Spec?.Context);
  }

  internal async Task<int> HandleAsync(CancellationToken cancellationToken)
  {
    var responseTask = _resourceProvisioner.ListKustomizationsWithHttpMessagesAsync();
    await foreach (var (_, kustomization) in responseTask.WatchAsync<V1CustomResourceDefinition, object>(cancellationToken: cancellationToken))
    {
      string? kustomizationName = kustomization?.Metadata.Name ??
        throw new InvalidOperationException("🚨 Kustomization name is null");
      var statusConditions = kustomization?.Status.Conditions;
      if (statusConditions is null)
      {
        _ = _kustomizations.Remove(kustomizationName);
        continue;
      }
      if (!_kustomizations.Add(kustomizationName))
      {
        if (_successfulKustomizations.Count == _kustomizations.Count)
        {
          return HandleSuccesfulKustomizations();
        }
        else if (_successfulKustomizations.Contains(kustomizationName))
        {
          continue;
        }
        else if (_stopwatch.Elapsed.TotalSeconds >= _config.Spec?.Timeout)
        {
          Console.WriteLine($"✕ Kustomization '{kustomizationName}' did not become ready within the specified time limit of {_config.Spec?.Timeout} seconds.");
          foreach (var statusCondition in statusConditions)
          {
            string? message = statusCondition.Message;
            Console.WriteLine(message);
            Console.WriteLine();
          }
          return 1;
        }
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
            return HandleFailedStatus(statusCondition, kustomizationName);
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
        HandleReadyStatus(kustomizationName);
      }
      else
      {
        HandleOtherStatus(kustomizationName);
      }
    }

    return 0;
  }

  static bool IsCritical(V1CustomResourceDefinitionCondition statusCondition) =>
    statusCondition.Status.Equals("False", StringComparison.Ordinal) &&
    !statusCondition.Reason.Equals("HealthCheckFailed", StringComparison.Ordinal);

  static bool HasDependencies(IList<V1CustomResourceDefinitionCondition> statusConditions) =>
    statusConditions.FirstOrDefault()?.Reason.Equals("DependencyNotReady", StringComparison.Ordinal) ?? false;

  int HandleSuccesfulKustomizations()
  {
    var totalTimeElapsed = _stopwatchTotal.Elapsed;
    int minutes = totalTimeElapsed.Minutes;
    int seconds = totalTimeElapsed.Seconds;
    Console.WriteLine($"✔ All kustomizations are ready! ({minutes}m {seconds}s)");
    return 0;
  }

  void HandleOtherStatus(string kustomizationName)
  {
    var timeElapsed = _stopwatch.Elapsed;
    int minutes = timeElapsed.Minutes;
    int seconds = timeElapsed.Seconds;
    string message = $"◎ Waiting for kustomization '{kustomizationName}' to become ready ({minutes}m {seconds}s)";
    if (message != _lastPrintedMessage)
    {
      Console.WriteLine(message);
      _lastPrintedMessage = message;
    }
  }

  void HandleReadyStatus(string kustomizationName)
  {
    var timeElapsed = _stopwatch.Elapsed;
    Console.WriteLine($"✔ Kustomization '{kustomizationName}' is ready! ({timeElapsed.Minutes}m {timeElapsed.Seconds}s)");
    _ = _successfulKustomizations.Add(kustomizationName);
    _stopwatch.Restart();
  }

  static int HandleFailedStatus(V1CustomResourceDefinitionCondition statusCondition, string kustomizationName)
  {
    string? message = statusCondition.Message;
    Console.WriteLine($"✕ Kustomization '{kustomizationName}' failed with message: {message}");
    return 1;
  }

  public void Dispose()
  {
    _resourceProvisioner.Dispose();
    _stopwatch.Stop();
    _stopwatchTotal.Stop();
  }
}
