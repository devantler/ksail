namespace KSail.Models.Kubernetes.FluxKustomization;

class FluxKustomizationPostBuild
{
  internal required List<FluxKustomizationPostBuildSubstituteFrom> SubstituteFrom { get; set; }
}
