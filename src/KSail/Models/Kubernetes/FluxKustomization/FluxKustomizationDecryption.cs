namespace KSail.Generators.Kubernetes.Models.FluxKustomization;

class FluxKustomizationDecryption
{
  internal required FluxKustomizationDecryptionProvider Provider { get; set; }
  internal required FluxKustomizationDecryptionSecretRef SecretRef { get; set; }
}
