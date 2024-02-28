using KSail.Generators.Models;

namespace KSail.Models.Kubernetes.FluxKustomization;

class FluxKustomization : IModel
{
  internal required List<FluxKustomizationContent> Content { get; set; } = [];
}
