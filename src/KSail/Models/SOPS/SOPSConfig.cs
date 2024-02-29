using KSail.Generators.Models;

namespace KSail.Models.K3d;

class SOPSConfig : IModel
{
  public required List<string> PublicKeys { get; set; }
}
