using System.CommandLine;
using System.Runtime.InteropServices;
using KSail.Commands.Root;

if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
  foreach (string file in Directory.GetFiles($"{AppContext.BaseDirectory}assets/binaries"))
  {
    File.SetUnixFileMode(file, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  }
}
var ksailCommand = new KSailRootCommand();
return await ksailCommand.InvokeAsync(args);
