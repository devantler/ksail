using System.CommandLine;
using KSail.Models;

namespace KSail.Options.Metadata;

/// <summary>
/// Options for metadata.
/// </summary>
/// <param name="config"></param>
public class MetadataOptions(KSailCluster config)
{
  /// <summary>
  /// The metadata name.
  /// </summary>
  public readonly MetadataNameOption NameOption = new(config) { Arity = ArgumentArity.ZeroOrOne };
}
