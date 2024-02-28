using KSail.Generators.Models;

namespace KSail.Generators.Kubernetes.Models.FluxKustomization;

class FluxKustomization : IModel
{
  internal required List<FluxKustomizationContent> Content { get; set; } = [];
}
