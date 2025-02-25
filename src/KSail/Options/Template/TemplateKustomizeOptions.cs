using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Template;

/// <summary>
/// Options for a template.
/// </summary>
/// <param name="config"></param>
public class TemplateKustomizeOptions(KSailCluster config)
{
  /// <summary>
  /// The kustomize root file.
  /// </summary>
  public readonly TemplateKustomizeRootOption RootOption = new(config) { Arity = ArgumentArity.ZeroOrOne };

  /// <summary>
  /// The kustomize flow options.
  /// </summary>
  public readonly TemplateKustomizeFlowOption FlowOption = new(config) { Arity = ArgumentArity.ZeroOrMore, AllowMultipleArgumentsPerToken = true };

  /// <summary>
  /// The kustomize hooks.
  /// </summary>
  public readonly TemplateKustomizeHookOption HookOption = new(config) { Arity = ArgumentArity.ZeroOrMore, AllowMultipleArgumentsPerToken = true };
}
