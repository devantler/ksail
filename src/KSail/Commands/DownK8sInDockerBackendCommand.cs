using System.CommandLine;
using System.Globalization;
using KSail.Enums;

namespace KSail.Commands;

/// <summary>
/// The 'down k8s-in-docker-backend' command responsible for destroying specific K8s clusters.
/// </summary>
public class DownK8sInDockerBackendCommand : Command
{
  /// <summary>
  /// Initializes a new instance of the <see cref="DownK8sInDockerBackendCommand"/> class.
  /// </summary>
  /// <param name="k8sInDockerBackend">An enum value representing the K8s-in-Docker backend.</param>
  /// <param name="nameOption">The -n, --name option.</param>
  public DownK8sInDockerBackendCommand(K8sInDockerBackend k8sInDockerBackend, Option<string> nameOption) : base(k8sInDockerBackend.ToString().ToLower(CultureInfo.InvariantCulture), "destroy a K8s cluster ")
  {
    AddOption(nameOption);

    this.SetHandler((name) =>
    {
      name = PromptName();

      Console.WriteLine($"🔥 Destroying '{name?.ToString()}' cluster...");
    }, nameOption);
  }

  static string PromptName()
  {
    string? name;
    do
    {
      Console.WriteLine("✍️ Please enter the name of the cluster to destroy:");
      Console.Write("> ");
      name = Console.ReadLine();
    }
    while (string.IsNullOrEmpty(name));
    return name;
  }
}
