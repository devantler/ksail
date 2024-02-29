namespace KSail.Models.Kubernetes;

class Kustomization
{
  public string Namespace { get; set; } = "";
  public required List<string> Resources { get; set; } = [];
}
