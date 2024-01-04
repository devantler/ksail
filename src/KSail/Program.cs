using System.CommandLine;
using KSail.Commands;

var rootCommand = new IntroductionRootCommand();
await rootCommand.InvokeAsync(args);
