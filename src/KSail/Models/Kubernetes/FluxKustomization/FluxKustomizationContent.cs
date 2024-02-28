namespace KSail.Generators.Kubernetes.Models.FluxKustomization;

class FluxKustomizationContent
{
  internal required string Name { get; set; }
  internal string Namespace { get; set; } = "flux-system";
  internal string Interval { get; set; } = "1m";
  internal List<string> DependsOn { get; set; } = [];
  internal FluxKustomizationSourceRef SourceRef { get; set; } = new FluxKustomizationSourceRef
  {
    Kind = "GitRepository",
    Name = "flux-system"
  };
  internal required string Path { get; set; }
  internal bool Prune { get; set; } = true;
  internal bool Wait { get; set; } = true;
  internal FluxKustomizationDecryption Decryption { get; set; } = new FluxKustomizationDecryption
  {
    Provider = FluxKustomizationDecryptionProvider.SOPS,
    SecretRef = new FluxKustomizationDecryptionSecretRef
    {
      Name = "sops-age"
    }
  };
  internal FluxKustomizationPostBuild PostBuild { get; set; } = new FluxKustomizationPostBuild
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
