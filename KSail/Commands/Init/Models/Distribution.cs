using System.Runtime.Serialization;

namespace KSail.Commands.Init.Models;

enum Distribution
{
  [EnumMember(Value = "k3d")]
  K3d
}
