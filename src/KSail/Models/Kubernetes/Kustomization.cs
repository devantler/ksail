using KSail.Generators.Models;

namespace KSail.Models.Kubernetes;

class Kustomization : IModel
{
  public string Namespace { get; set; } = "";
  public required List<string> Resources { get; set; } = [];
}
