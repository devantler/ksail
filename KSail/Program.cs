# pragma warning disable CA1031
using System.CommandLine;
using KSail.Commands.Root;

var ksailCommand = new KSailRootCommand();
return await ksailCommand.InvokeAsync(args).ConfigureAwait(false);

