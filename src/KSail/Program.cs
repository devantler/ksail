
using System.Diagnostics.CodeAnalysis;
using KSail;

[ExcludeFromCodeCoverage]
class Program
{
  static async Task<int> Main(string[] args)
  {
    var startup = new Startup();
    return await startup.RunAsync(args);
  }
}
