using System.CommandLine;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using k8s;
using k8s.Models;
using KSail.Extensions;

namespace KSail.Commands.Check.Handlers;
class KSailCheckCommandHandler(IConsole console)
{
  static readonly HashSet<string> kustomizations = [];
  static readonly HashSet<string> successFullKustomizations = [];
  static readonly Stopwatch stopwatch = Stopwatch.StartNew();
  readonly IConsole console = console;

  internal async Task HandleAsync(string name, int timeout, CancellationToken cancellationToken)
  {
    console.WriteLine("👀 Checking the status the cluster...");
    var kubernetesClient = CreateKubernetesClientFromClusterName(name);
    var responseTask = kubernetesClient.ListKustomizationsWithHttpMessagesAsync(cancellationToken);

    await foreach (var (type, kustomization) in responseTask.WatchAsync<V1CustomResourceDefinition, object>(cancellationToken: cancellationToken))
    {
      string? kustomizationName = kustomization?.Metadata.Name ??
        throw new NoNullAllowedException("Kustomization name is null");
      string? statusName = kustomization?.Status.Conditions.FirstOrDefault()?.Type ??
        throw new NoNullAllowedException("Kustomization status is null");

      if (!kustomizations.Add(kustomizationName))
      {
        if (successFullKustomizations.Count == kustomizations.Count)
        {
          console.WriteLine("✔ All kustomizations are ready!");
          Environment.Exit(0);
        }
        else if (stopwatch.Elapsed.TotalSeconds >= timeout)
        {
          console.WriteLine($"✕ Timeout reached. Kustomization '{kustomizationName}' did not become ready within the specified timeout of {timeout} seconds.");
          Environment.Exit(1);
        }
        else if (successFullKustomizations.Contains(kustomizationName))
        {
          continue;
        }
      }

      switch (statusName)
      {
        case "Failed":
          HandleFailedStatus(kustomization, kustomizationName);
          break;
        case "Ready":
          HandleReadyStatus(kustomizationName);
          break;
        default:
          console.WriteLine($"► Waiting for kustomization '{kustomizationName}' to be ready. It is currently {statusName?.ToLower(CultureInfo.InvariantCulture)}...");
          break;
      }
    }
  }

  void HandleReadyStatus(string kustomizationName)
  {
    console.WriteLine($"✔ Kustomization '{kustomizationName}' is ready!");
    _ = successFullKustomizations.Add(kustomizationName);
    stopwatch.Restart();
  }

  void HandleFailedStatus(V1CustomResourceDefinition kustomization, string kustomizationName)
  {
    console.WriteLine($"✕ Kustomization '{kustomizationName}' failed!");
    string? message = kustomization?.Status.Conditions.FirstOrDefault()?.Message;
    console.WriteLine($"✕ {message}");
    Environment.Exit(1);
  }

  Kubernetes CreateKubernetesClientFromClusterName(string name)
  {
    var kubeConfig = KubernetesClientConfiguration.LoadKubeConfig();
    var context = kubeConfig.Contexts.FirstOrDefault(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

    if (context == null)
    {
      console.WriteLine($"✕ Could not find a context matching the cluster name '{name}' in the kubeconfig file.");
      console.WriteLine($"   Available contexts are: {string.Join(", ", kubeConfig.Contexts.Select(c => c.Name))}");
      Environment.Exit(1);
    }
    var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(kubeConfig, context.Name);
    return new Kubernetes(config);
  }
}
