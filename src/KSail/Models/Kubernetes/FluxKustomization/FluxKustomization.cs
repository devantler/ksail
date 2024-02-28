using KSail.Generators.Models;

namespace KSail.Models.Kubernetes.FluxKustomization;

class FluxKustomization : IModel
{
  public required List<FluxKustomizationContent> Content { get; set; } = [];
}
