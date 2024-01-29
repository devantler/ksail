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
  Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/ksail_sops.agekey");
  foreach (string file in Directory.GetFiles($"{AppContext.BaseDirectory}assets/binaries"))
  {
    File.SetUnixFileMode(file, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  }
  var ksailCommand = new KSailRootCommand();
  exitCode = await ksailCommand.InvokeAsync(args);
}
return exitCode;
