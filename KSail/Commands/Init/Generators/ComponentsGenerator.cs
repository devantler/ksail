using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using Devantler.KubernetesGenerator.Kustomize.Models.Patches;

namespace KSail.Commands.Init.Generators;

class ComponentsGenerator
{
  readonly KustomizeComponentGenerator _kustomizeComponentKubernetesGenerator = new();

  internal async Task GenerateAsync(string outputPath)
  {
    string componentsPath = Path.Combine(outputPath, "components");
    if (!Directory.Exists(componentsPath))
    {
      _ = Directory.CreateDirectory(componentsPath);
    }
    await GenerateFluxKustomizationPostBuildVariablesLabelComponent(componentsPath).ConfigureAwait(false);
    await GenerateFluxKustomizationSOPSLabelComponent(componentsPath).ConfigureAwait(false);
    await GenerateHelmReleaseCRDSLabelComponent(componentsPath).ConfigureAwait(false);
    await GenerateHelmReleaseRemediationLabelComponent(componentsPath).ConfigureAwait(false);
  }

  async Task GenerateFluxKustomizationPostBuildVariablesLabelComponent(string outputPath)
  {
    string fluxKustomizationPostBuildVariablesLabelComponentPath = Path.Combine(outputPath, "flux-kustomization-post-build-variables-label");
    if (!Directory.Exists(fluxKustomizationPostBuildVariablesLabelComponentPath))
    {
      _ = Directory.CreateDirectory(fluxKustomizationPostBuildVariablesLabelComponentPath);
    }
    string fluxKustomizationPostBuildVariablesLabelComponentKustomizationPath = Path.Combine(fluxKustomizationPostBuildVariablesLabelComponentPath, "kustomization.yaml");
    if (File.Exists(fluxKustomizationPostBuildVariablesLabelComponentKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{fluxKustomizationPostBuildVariablesLabelComponentKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{fluxKustomizationPostBuildVariablesLabelComponentKustomizationPath}'");
    var fluxKustomizationPostBuildVariablesLabelComponent = new KustomizeComponent
    {
      Patches =
      [
        new KustomizePatch
        {
          Target = new KustomizeTarget
          {
            Kind = "Kustomization",
            LabelSelector = "kustomize.toolkit.fluxcd.io/post-build-variables=enabled"
          },
          Patch = """
          apiVersion: kustomize.toolkit.fluxcd.io/v1
          kind: Kustomization
          metadata:
            name: all
          spec:
            postBuild:
              substituteFrom:
              - kind: ConfigMap
                name: variables-cluster
              - kind: Secret
                name: variables-sensitive-cluster
              - kind: ConfigMap
                name: variables-distribution
              - kind: Secret
                name: variables-sensitive-distribution
              - kind: ConfigMap
                name: variables-global
              - kind: Secret
                name: variables-sensitive-global
          """
        }
      ]
    };
    await _kustomizeComponentKubernetesGenerator.GenerateAsync(fluxKustomizationPostBuildVariablesLabelComponent, fluxKustomizationPostBuildVariablesLabelComponentKustomizationPath).ConfigureAwait(false);
  }

  async Task GenerateFluxKustomizationSOPSLabelComponent(string outputPath)
  {
    string fluxKustomizationSOPSLabelComponentPath = Path.Combine(outputPath, "flux-kustomization-sops-label");
    if (!Directory.Exists(fluxKustomizationSOPSLabelComponentPath))
    {
      _ = Directory.CreateDirectory(fluxKustomizationSOPSLabelComponentPath);
    }
    string fluxKustomizationSOPSLabelComponentKustomizationPath = Path.Combine(fluxKustomizationSOPSLabelComponentPath, "kustomization.yaml");
    if (File.Exists(fluxKustomizationSOPSLabelComponentKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{fluxKustomizationSOPSLabelComponentKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{fluxKustomizationSOPSLabelComponentKustomizationPath}'");
    var fluxKustomizationSOPSLabelComponent = new KustomizeComponent
    {
      Patches =
      [
        new KustomizePatch
        {
          Target = new KustomizeTarget
          {
            Kind = "Kustomization",
            LabelSelector = "kustomize.toolkit.fluxcd.io=enabled"
          },
          Patch = """
          apiVersion: kustomize.toolkit.fluxcd.io/v1
          kind: Kustomization
          metadata:
            name: all
          spec:
            decryption:
            provider: sops
            secretRef:
              name: sops-age
          """
        }
      ]
    };
    await _kustomizeComponentKubernetesGenerator.GenerateAsync(fluxKustomizationSOPSLabelComponent, fluxKustomizationSOPSLabelComponentKustomizationPath).ConfigureAwait(false);
  }

  async Task GenerateHelmReleaseCRDSLabelComponent(string outputPath)
  {
    string helmReleaseCRDSLabelComponentPath = Path.Combine(outputPath, "helm-release-crds-label");
    if (!Directory.Exists(helmReleaseCRDSLabelComponentPath))
    {
      _ = Directory.CreateDirectory(helmReleaseCRDSLabelComponentPath);
    }
    string helmReleaseCRDSLabelComponentKustomizationPath = Path.Combine(helmReleaseCRDSLabelComponentPath, "kustomization.yaml");
    if (File.Exists(helmReleaseCRDSLabelComponentKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{helmReleaseCRDSLabelComponentKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{helmReleaseCRDSLabelComponentKustomizationPath}'");
    var helmReleaseCRDSLabelComponent = new KustomizeComponent
    {
      Patches =
      [
        new KustomizePatch
        {
          Target = new KustomizeTarget
          {
            Kind = "HelmRelease",
            LabelSelector = "helm.toolkit.fluxcd.io/crds=enabled"
          },
          Patch = """
          apiVersion: helm.toolkit.fluxcd.io/v2
          kind: HelmRelease
          metadata:
            name: all
          spec:
            install:
              crds: CreateReplace
            upgrade:
              crds: CreateReplace
          """
        }
      ]
    };
    await _kustomizeComponentKubernetesGenerator.GenerateAsync(helmReleaseCRDSLabelComponent, helmReleaseCRDSLabelComponentKustomizationPath).ConfigureAwait(false);
  }

  async Task GenerateHelmReleaseRemediationLabelComponent(string outputPath)
  {
    string helmReleaseRemediationLabelComponentPath = Path.Combine(outputPath, "helm-release-remediation-label");
    if (!Directory.Exists(helmReleaseRemediationLabelComponentPath))
    {
      _ = Directory.CreateDirectory(helmReleaseRemediationLabelComponentPath);
    }
    string helmReleaseRemediationLabelComponentKustomizationPath = Path.Combine(helmReleaseRemediationLabelComponentPath, "kustomization.yaml");
    if (File.Exists(helmReleaseRemediationLabelComponentKustomizationPath))
    {
      Console.WriteLine($"✔ Skipping '{helmReleaseRemediationLabelComponentKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ Generating '{helmReleaseRemediationLabelComponentKustomizationPath}'");
    var helmReleaseRemediationLabelComponent = new KustomizeComponent
    {
      Patches =
      [
        new KustomizePatch
        {
          Target = new KustomizeTarget
          {
            Kind = "HelmRelease",
            LabelSelector = "helm.toolkit.fluxcd.io/remediation=enabled"
          },
          Patch = """
          apiVersion: helm.toolkit.fluxcd.io/v2
          kind: HelmRelease
          metadata:
            name: all
          spec:
            install:
              remediation:
                retries: 10
            upgrade:
              remediation:
                retries: 10
          """
        }
      ]
    };
    await _kustomizeComponentKubernetesGenerator.GenerateAsync(helmReleaseRemediationLabelComponent, helmReleaseRemediationLabelComponentKustomizationPath).ConfigureAwait(false);
  }
}
