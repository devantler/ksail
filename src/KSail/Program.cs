using System.CommandLine;
using KSail.Commands;

var ksailCommand = new KSailCommand();
await ksailCommand.InvokeAsync(args);
