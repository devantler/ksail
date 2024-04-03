namespace KSail.Models.Kubernetes.FluxKustomization;

class FluxKustomizationDecryption
{
  public required FluxKustomizationDecryptionProvider Provider { get; set; }
  public required FluxKustomizationDecryptionSecretRef SecretRef { get; set; }
}
