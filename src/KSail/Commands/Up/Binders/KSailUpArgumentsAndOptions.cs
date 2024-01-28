namespace KSail.Commands.Up.Binders;

class KSailUpArgumentsAndOptions
{
  public required string ClusterName { get; set; }
  public required string Config { get; set; }
  public required string Manifests { get; set; }
  public required string Kustomizations { get; set; }
  public int Timeout { get; set; }
  public bool NoSOPS { get; set; }
}
