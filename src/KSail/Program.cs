using System.CommandLine;
using KSail.Commands;

//Make all binaries in AppContext.BaseDirectory/assets executable
foreach (string file in Directory.GetFiles($"{AppContext.BaseDirectory}assets/binaries"))
{
  //
  File.SetUnixFileMode(file, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
}

var ksailCommand = new KSailCommand();
await ksailCommand.InvokeAsync(args);
