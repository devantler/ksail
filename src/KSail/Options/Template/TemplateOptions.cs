using KSail.Models;

namespace KSail.Options.Template;



internal class TemplateOptions(KSailCluster config)
{

  public readonly TemplateKustomizeOptions Kustomize = new(config);
}
