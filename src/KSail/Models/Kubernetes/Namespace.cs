using KSail.Generators.Models;

namespace KSail.Models.Kubernetes;

class Namespace : IModel
{
  public required string Name { get; set; }
}
