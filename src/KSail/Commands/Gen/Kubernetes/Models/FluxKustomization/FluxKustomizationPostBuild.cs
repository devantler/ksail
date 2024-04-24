namespace KSail.Models.Kubernetes.FluxKustomization;

class FluxKustomizationPostBuild
{
  public required List<FluxKustomizationPostBuildSubstituteFrom> SubstituteFrom { get; set; }
}
