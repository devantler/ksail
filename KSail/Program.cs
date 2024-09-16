using System.CommandLine;
using KSail.Commands.Root;

var ksailCommand = new KSailRootCommand();
int exitCode = await ksailCommand.InvokeAsync(args).ConfigureAwait(false);
return exitCode;

