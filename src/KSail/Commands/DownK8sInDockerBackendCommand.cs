using System.CommandLine;
using System.Globalization;
using KSail.Enums;
using Spectre.Console;

namespace KSail.Commands;

public class DownBackendCommand : Command
{
  public DownBackendCommand(K8sInDockerBackend k8sInDockerBackend, Option<string> nameOption) : base(k8sInDockerBackend.ToString().ToLower(CultureInfo.InvariantCulture), "destroy a K8s cluster in Docker")
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
