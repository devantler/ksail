using KSail.Models.CLI.Commands;
using KSail.Models.CLI.Commands.Init;
using KSail.Models.CLI.Commands.Sops;

namespace KSail.Models.CLI;

/// <summary>
/// The options to use for the KSail CLI commands.
/// </summary>
public class KSailCLIOptions
{
  /// <summary>
  /// The options to use for the 'check' command.
  /// </summary>
  public KSailCLICheckOptions CheckOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'debug' command.
  /// </summary>
  public KSailCLIDebugOptions DebugOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'down' command.
  /// </summary>
  public KSailCLIDownOptions DownOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'gen' command.
  /// </summary>
  public KSailCLIGenOptions GenOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'init' command.
  /// </summary>
  public KSailCLIInitOptions InitOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'lint' command.
  /// </summary>
  public KSailCLILintOptions LintOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'list' command.
  /// </summary>
  public KSailCLIListOptions ListOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'sops' command.
  /// </summary>
  public KSailCLISopsOptions SopsOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'start' command.
  /// </summary>
  public KSailCLIStartOptions StartOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'stop' command.
  /// </summary>
  public KSailCLIStopOptions StopOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'up' command.
  /// </summary>
  public KSailCLIUpOptions UpOptions { get; set; } = new();

  /// <summary>
  /// The options to use for the 'update' command.
  /// </summary>
  public KSailCLIUpdateOptions UpdateOptions { get; set; } = new();
}
