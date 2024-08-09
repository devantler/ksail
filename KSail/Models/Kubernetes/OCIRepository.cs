namespace KSail.Models.Kubernetes;

class OCIRepository
{
  public required string Name { get; set; }
  public required string Namespace { get; set; }
  public string Interval { get; set; } = "1m";
  public required string Url { get; set; }
  public required string Tag { get; set; }
}
