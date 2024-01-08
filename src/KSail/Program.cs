using System.CommandLine;
using System.Runtime.InteropServices;
using KSail.Commands;

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
  Console.WriteLine("🚨 Windows is not supported.");
  Environment.Exit(1);
}
else
{
  foreach (string file in Directory.GetFiles($"{AppContext.BaseDirectory}assets/binaries"))
  {
    File.SetUnixFileMode(file, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  }

  var ksailCommand = new KSailCommand();
  _ = await ksailCommand.InvokeAsync(args);
}
