using Devantler.KubernetesGenerator.Kustomize;
using Devantler.KubernetesGenerator.Kustomize.Models;
using Devantler.KubernetesGenerator.Kustomize.Models.Patches;
using KSail.Models;
using KSail.Models.Commands.Init;

namespace KSail.Commands.Init.Generators.SubGenerators;

class ComponentsGenerator
{
  readonly KustomizeComponentGenerator _kustomizeComponentKubernetesGenerator = new();

  internal async Task GenerateAsync(KSailCluster config, CancellationToken cancellationToken = default)
  {
    string componentsPath = Path.Combine(config.Spec.InitOptions.OutputDirectory, "k8s/components");
    if (!Directory.Exists(componentsPath))
      _ = Directory.CreateDirectory(componentsPath);
    if (config.Spec.InitOptions.Template == KSailInitTemplate.Simple)
    {
      if (config.Spec.InitOptions.PostBuildVariables)
      {
        await GenerateFluxKustomizationPostBuildVariablesLabelComponent(config, componentsPath, cancellationToken).ConfigureAwait(false);
      }
      await GenerateHelmReleaseCRDSLabelComponent(componentsPath, cancellationToken).ConfigureAwait(false);
      await GenerateHelmReleaseRemediationLabelComponent(componentsPath, cancellationToken).ConfigureAwait(false);
    }
    if (config.Spec.SOPS)
      await GenerateFluxKustomizationSOPSLabelComponent(componentsPath, cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateFluxKustomizationPostBuildVariablesLabelComponent(KSailCluster config, string outputPath, CancellationToken cancellationToken = default)
  {
    string fluxKustomizationPostBuildVariablesLabelComponentPath = Path.Combine(outputPath, "flux-kustomization-post-build-variables-label");
    if (!Directory.Exists(fluxKustomizationPostBuildVariablesLabelComponentPath))
      _ = Directory.CreateDirectory(fluxKustomizationPostBuildVariablesLabelComponentPath);
    string fluxKustomizationPostBuildVariablesLabelComponentKustomizationPath = Path.Combine(fluxKustomizationPostBuildVariablesLabelComponentPath, "kustomization.yaml");
    if (File.Exists(fluxKustomizationPostBuildVariablesLabelComponentKustomizationPath))
    {
      Console.WriteLine($"✔ skipping '{fluxKustomizationPostBuildVariablesLabelComponentKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{fluxKustomizationPostBuildVariablesLabelComponentKustomizationPath}'");
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
          """
        }
      ]
    };
    foreach (string hook in config.Spec.InitOptions.KustomizeHooks.Skip(1))
    {
      fluxKustomizationPostBuildVariablesLabelComponent.Patches.First().Patch += $"{Environment.NewLine}    - kind: ConfigMap";
      fluxKustomizationPostBuildVariablesLabelComponent.Patches.First().Patch += $"{Environment.NewLine}      name: variables-{hook}-cluster";
      fluxKustomizationPostBuildVariablesLabelComponent.Patches.First().Patch += $"{Environment.NewLine}    - kind: Secret";
      fluxKustomizationPostBuildVariablesLabelComponent.Patches.First().Patch += $"{Environment.NewLine}      name: variables-sensitive-{hook}-cluster";
    }
    await _kustomizeComponentKubernetesGenerator.GenerateAsync(fluxKustomizationPostBuildVariablesLabelComponent, fluxKustomizationPostBuildVariablesLabelComponentKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateFluxKustomizationSOPSLabelComponent(string outputPath, CancellationToken cancellationToken = default)
  {
    string fluxKustomizationSOPSLabelComponentPath = Path.Combine(outputPath, "flux-kustomization-sops-label");
    if (!Directory.Exists(fluxKustomizationSOPSLabelComponentPath))
      _ = Directory.CreateDirectory(fluxKustomizationSOPSLabelComponentPath);
    string fluxKustomizationSOPSLabelComponentKustomizationPath = Path.Combine(fluxKustomizationSOPSLabelComponentPath, "kustomization.yaml");
    if (File.Exists(fluxKustomizationSOPSLabelComponentKustomizationPath))
    {
      Console.WriteLine($"✔ skipping '{fluxKustomizationSOPSLabelComponentKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{fluxKustomizationSOPSLabelComponentKustomizationPath}'");
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
    await _kustomizeComponentKubernetesGenerator.GenerateAsync(fluxKustomizationSOPSLabelComponent, fluxKustomizationSOPSLabelComponentKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateHelmReleaseCRDSLabelComponent(string outputPath, CancellationToken cancellationToken = default)
  {
    string helmReleaseCRDSLabelComponentPath = Path.Combine(outputPath, "helm-release-crds-label");
    if (!Directory.Exists(helmReleaseCRDSLabelComponentPath))
      _ = Directory.CreateDirectory(helmReleaseCRDSLabelComponentPath);
    string helmReleaseCRDSLabelComponentKustomizationPath = Path.Combine(helmReleaseCRDSLabelComponentPath, "kustomization.yaml");
    if (File.Exists(helmReleaseCRDSLabelComponentKustomizationPath))
    {
      Console.WriteLine($"✔ skipping '{helmReleaseCRDSLabelComponentKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{helmReleaseCRDSLabelComponentKustomizationPath}'");
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
    await _kustomizeComponentKubernetesGenerator.GenerateAsync(helmReleaseCRDSLabelComponent, helmReleaseCRDSLabelComponentKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  async Task GenerateHelmReleaseRemediationLabelComponent(string outputPath, CancellationToken cancellationToken = default)
  {
    string helmReleaseRemediationLabelComponentPath = Path.Combine(outputPath, "helm-release-remediation-label");
    if (!Directory.Exists(helmReleaseRemediationLabelComponentPath))
      _ = Directory.CreateDirectory(helmReleaseRemediationLabelComponentPath);
    string helmReleaseRemediationLabelComponentKustomizationPath = Path.Combine(helmReleaseRemediationLabelComponentPath, "kustomization.yaml");
    if (File.Exists(helmReleaseRemediationLabelComponentKustomizationPath))
    {
      Console.WriteLine($"✔ skipping '{helmReleaseRemediationLabelComponentKustomizationPath}', as it already exists.");
      return;
    }
    Console.WriteLine($"✚ generating '{helmReleaseRemediationLabelComponentKustomizationPath}'");
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
    await _kustomizeComponentKubernetesGenerator.GenerateAsync(helmReleaseRemediationLabelComponent, helmReleaseRemediationLabelComponentKustomizationPath, cancellationToken: cancellationToken).ConfigureAwait(false);
  }
}
