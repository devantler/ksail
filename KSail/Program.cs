using System.CommandLine;
using System.Runtime.InteropServices;
using KSail.Commands.Root;

int exitCode = 0;
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
  Console.WriteLine("🚨 This application is not supported on Windows.");
  Environment.Exit(1);
}
else
{
  foreach (string file in Directory.GetFiles($"{AppContext.BaseDirectory}assets/binaries"))
  {
    File.SetUnixFileMode(file, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  }
  var ksailCommand = new KSailRootCommand();
  exitCode = await ksailCommand.InvokeAsync(args).ConfigureAwait(false);
}
return exitCode;
