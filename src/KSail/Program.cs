using System.CommandLine;
using KSail.Presentation.Commands;

var ksailCommand = new KSailCommand();
await ksailCommand.InvokeAsync(args);
