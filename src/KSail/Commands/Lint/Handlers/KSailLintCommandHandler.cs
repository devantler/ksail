using System.CommandLine.Invocation;

namespace KSail.Commands.Lint.Handlers;

class KSailLintCommandHandler
{
  internal static async Task Handle(string manifestsPath)
  {
    Console.WriteLine($"Linting files in '{manifestsPath}'...");

  }
}
