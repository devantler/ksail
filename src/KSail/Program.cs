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
  // Set bundle extraction directory to ~/.ksail/assets/binaries
  Environment.SetEnvironmentVariable("DOTNET_BUNDLE_EXTRACT_BASE_DIR", $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail");
  Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.ksail/ksail_sops.agekey");
  foreach (string file in Directory.GetFiles($"{AppContext.BaseDirectory}assets/binaries"))
  {
    File.SetUnixFileMode(file, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  }

  var ksailCommand = new KSailCommand();
  _ = await ksailCommand.InvokeAsync(args);
}
