namespace KSail.Models.Kubernetes.FluxKustomization;

class FluxKustomization
{
  public required List<FluxKustomizationContent> Content { get; set; } = [];
}
