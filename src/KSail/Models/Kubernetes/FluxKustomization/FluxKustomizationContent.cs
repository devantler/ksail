namespace KSail.Models.Kubernetes.FluxKustomization;

class FluxKustomizationContent
{
  public required string Name { get; set; }
  public string Namespace { get; set; } = "flux-system";
  public string Interval { get; set; } = "1m";
  public List<string> DependsOn { get; set; } = [];
  public FluxKustomizationSourceRef SourceRef { get; set; } = new FluxKustomizationSourceRef
  {
    Kind = "OCIRepository",
    Name = "flux-system"
  };
  public required string Path { get; set; }
  public bool Prune { get; set; } = true;
  public bool Wait { get; set; } = true;
  public FluxKustomizationDecryption Decryption { get; set; } = new FluxKustomizationDecryption
  {
    Provider = FluxKustomizationDecryptionProvider.SOPS,
    SecretRef = new FluxKustomizationDecryptionSecretRef
    {
      Name = "sops-age"
    }
  };
  public FluxKustomizationPostBuild PostBuild { get; set; } = new FluxKustomizationPostBuild
  {
    SubstituteFrom =
    [
      new() {
        Kind = "ConfigMap",
        Name = "variables"
      },
      new() {
        Kind = "Secret",
        Name = "variables-sensitive"
      }
      ]
  };
}
