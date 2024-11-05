> [!NOTE]
> Ahoy, matey! Gather 'round and lend an ear, for a grand overhaul be on the horizon! All them embedded binaries be set to sail into their own .NET project, makin' it smoother fer me to keep 'em shipshape. But beware, this mighty endeavor means KSail won't be seein' any bug fixes or new features 'til the midst of November, as this here voyage will take a few moons to chart right.
>
> Keep yer spyglass fixed on the progress at:
>
> - [ ] [devantler/dotnet-sops-cli](https://github.com/devantler/dotnet-sops-cli)
> - [x] [devantler/dotnet-age-cli](https://github.com/devantler/dotnet-age-cli)
> - [x] [devantler/dotnet-cli-runner](https://github.com/devantler/dotnet-cli-runner)
> - [x] [devantler/dotnet-container-engine-provisioner](https://github.com/devantler/dotnet-container-engine-provisioner)
> - [x] [devantler/dotnet-flux-cli](https://github.com/devantler/dotnet-flux-cli)
> - [x] [devantler/dotnet-k3d-cli](https://github.com/devantler/dotnet-k3d-cli)
> - [x] [devantler/dotnet-k9s-cli](https://github.com/devantler/dotnet-k9s-cli)
> - [x] [devantler/dotnet-key-manager](https://github.com/devantler/dotnet-sops-manager)
> - [x] [devantler/dotnet-keys](https://github.com/devantler/dotnet-keys)
> - [x] [devantler/dotnet-kind-cli](https://github.com/devantler/dotnet-kind-cli)
> - [x] [devantler/dotnet-kubeconform-cli](https://github.com/devantler/dotnet-kubeconform-cli)
> - [x] [devantler/dotnet-kubernetes-generator](https://github.com/devantler/dotnet-kubernetes-generator)
> - [x] [devantler/dotnet-kubernetes-provisioner](https://github.com/devantler/dotnet-kubernetes-provisioner)
> - [x] [devantler/dotnet-kustomize-cli](https://github.com/devantler/dotnet-kustomize-cli)
> - [x] [devantler/dotnet-template-engine](https://github.com/devantler/dotnet-template-engine)

<div align="center">
  <img width="400px" alt="ksail" align="center" src="./wiki/images/ksail-logo.png" />
</div>

# 🛥️🐳 KSail

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![Test](https://github.com/devantler/ksail/actions/workflows/test.yaml/badge.svg?branch=main)](https://github.com/devantler/ksail/actions/workflows/test.yaml)
[![codecov](https://codecov.io/gh/devantler/ksail/graph/badge.svg?token=DNEO90PfNR)](https://codecov.io/gh/devantler/ksail)

![image](https://github.com/devantler/ksail/assets/26203420/2c4596bd-68e5-438f-9a8b-0626bb44f353)

<details>
  <summary>Show/hide folder structure</summary>

<!-- readme-tree start -->

```
.
├── .github
│   └── workflows
├── .vscode
├── KSail.Models
│   ├── Commands
│   │   ├── Check
│   │   ├── Debug
│   │   ├── Down
│   │   ├── Gen
│   │   ├── Init
│   │   ├── Lint
│   │   ├── List
│   │   ├── Sops
│   │   ├── Start
│   │   ├── Stop
│   │   ├── Up
│   │   └── Update
│   └── Registry
├── docs
│   └── images
├── scripts
├── src
│   └── KSail
│       ├── Arguments
│       ├── CLIWrappers
│       ├── Commands
│       │   ├── Check
│       │   │   └── Handlers
│       │   ├── Debug
│       │   │   └── Handlers
│       │   ├── Down
│       │   │   ├── Handlers
│       │   │   └── Options
│       │   ├── Init
│       │   │   ├── Generators
│       │   │   └── Handlers
│       │   ├── Lint
│       │   │   └── Handlers
│       │   ├── List
│       │   │   └── Handlers
│       │   ├── Root
│       │   │   └── Handlers
│       │   ├── SOPS
│       │   │   ├── Handlers
│       │   │   └── Options
│       │   ├── Start
│       │   │   └── Handlers
│       │   ├── Stop
│       │   │   └── Handlers
│       │   ├── Up
│       │   │   ├── Handlers
│       │   │   └── Options
│       │   └── Update
│       │       ├── Handlers
│       │       └── Options
│       ├── Enums
│       ├── Extensions
│       ├── Models
│       │   ├── K3d
│       │   ├── KSail
│       │   ├── Kubernetes
│       │   │   └── FluxKustomization
│       │   └── SOPS
│       ├── Options
│       ├── Provisioners
│       │   ├── ContainerEngine
│       │   ├── ContainerOrchestrator
│       │   ├── GitOps
│       │   ├── KubernetesDistribution
│       │   └── SecretManager
│       └── assets
│           ├── binaries
│           └── templates
│               ├── k3d
│               ├── kubernetes
│               └── sops
└── tests
    └── KSail.Tests
        ├── Commands
        │   ├── Check
        │   ├── Debug
        │   ├── Down
        │   ├── Lint
        │   ├── List
        │   ├── Root
        │   ├── SOPS
        │   ├── Up
        │   └── Update
        └── TestUtils

89 directories
```

<!-- readme-tree end -->

</details>

## Getting Started

### Prerequisites

> [!NOTE]
> On MacOS (darwin), ye need to "Allow the default Docker socket to be used (requires password)" in Docker Desktop settings.
>
> <details><summary>Show me how, ye scallywag!</summary>
>
> ![Enable Docker Socket in Docker Desktop](docs/images/enable-docker-socket-in-docker-desktop.png)
>
> </details>

KSail supports MacOS and Linux on the followin' platforms:

- darwin-amd64 
- darwin-arm64 
- linux-amd64 🐧
- linux-arm64 🐧

If ye be usin' Windows, ye can use WSL2 to run KSail.

### Installation

With Homebrew:

```sh
brew tap devantler/formulas
brew install ksail
```

Manually:

1. Download the latest release from the [releases page](https://github.com/devantler/ksail/releases).
2. Make the binary executable: `chmod +x ksail`.
3. Move the binary to a directory in yer `$PATH`: `mv ksail /usr/local/bin/ksail`.

### 📝 Usage

Gettin' started with KSail be as easy as plunderin' treasure. Here be a few commands to get ye goin':

> `ksail init <name-of-cluster>` - To initialize yer cluster.
>
> `ksail up <name-of-cluster>` - To provision yer cluster.

From there, ye can make some changes to yer manifest files, and when ye be ready to apply 'em, ye can run:

> `ksail update <name-of-cluster>` - To update yer cluster.

At some point, ye might encounter an issue and wonder what be goin' on. In that case, ye can run:

> `ksail check` - To check the status of yer cluster reconciliations.

And for more advanced debuggin', ye can run:

> `ksail debug` - To debug yer cluster with the K9s tool.

Finally, when ye be done workin' with yer cluster, ye can run:

> `ksail stop <name-of-cluster>` - To stop yer cluster, so ye can continue workin' on it later.

Or if ye really want to get rid of it for now, ye can run:

> `ksail down <name-of-cluster>` - To dismantle yer cluster and remove its resources.

## Documentation

> [!NOTE]
> The documentation be a work in progress. When it be more mature, it will be made available on <ksail.devantler.com> and on the Wiki.
> For now, it includes the information that was originally available in this README with a few additions.

- [Overview](./wiki/0-overview.md)
- [Getting Started](./wiki/1-getting-started.md)
- [Configuration](./wiki/2-configuration.md)
- [Structure](./wiki/3-structure.md)
- [CI](./wiki/4-ci.md)
- [KSail CLI](./wiki/5-ksail-cli.md)
- [Supported Tooling](./wiki/6-supported-tooling.md)
- [FAQ](./wiki/7-faq.md)
- [Roadmap](./wiki/8-roadmap.md)

## Sub-projects

- **[devantler/dotnet-age-cli](https://github.com/devantler/dotnet-age-cli)** - A library that embeds and provides an API for the Age CLI.
- **[devantler/dotnet-cli-runner](https://github.com/devantler/dotnet-cli-runner)** - An implementation atop CLI Wrap to support runnin' different binaries from C# code.
- **[devantler/dotnet-container-engine-provisioner](https://github.com/devantler/dotnet-container-engine-provisioner)** - Provisioners to provision resources in container engines like Docker or Podman.
- **[devantler/dotnet-flux-cli](https://github.com/devantler/dotnet-flux-cli)** - A library that embeds and provides an API for the Flux CLI.
- **[devantler/dotnet-k3d-cli](https://github.com/devantler/dotnet-k3d-cli)** - A library that embeds and provides an API for the K3d CLI.
- **[devantler/dotnet-k9s-cli](https://github.com/devantler/dotnet-k9s-cli)** - A library that embeds and provides an API for the K9s CLI.
- **[devantler/dotnet-key-manager](https://github.com/devantler/dotnet-key-manager)** - A key manager to guard yer local Age keys.
- **[devantler/dotnet-keys](https://github.com/devantler/dotnet-keys)** - A library with key models, like the Age key model.
- **[devantler/dotnet-kind-cli](https://github.com/devantler/dotnet-kind-cli)** - A library that embeds and provides an API for the Kind CLI.
- **[devantler/dotnet-kubeconform-cli](https://github.com/devantler/dotnet-kubeconform-cli)** - A library that embeds and provides an API for the Kubeconform CLI.
- **[devantler/dotnet-kubernetes-generator](https://github.com/devantler/dotnet-kubernetes-generator)** - Generators to conjure Kubernetes resources with `ksail init` and `ksail gen`.
- **[devantler/dotnet-kubernetes-provisioner](https://github.com/devantler/dotnet-kubernetes-provisioner)** - Provisioners to provision Kubernetes clusters and resources.
- **[devantler/dotnet-kubernetes-validator](https://github.com/devantler/dotnet-kubernetes-validator)** - A library that validates Kubernetes resources client-side or at runtime.
- **[devantler/dotnet-kustomize-cli](https://github.com/devantler/dotnet-kustomize-cli)** - A library that embeds and provides an API for the Kustomize CLI.
- **[devantler/dotnet-sops-cli](https://github.com/devantler/dotnet-sops-cli)** - A library that embeds and provides an API for the SOPS CLI.
- **[devantler/dotnet-template-engine](https://github.com/devantler/dotnet-template-engine)** - A template engine to support code generation of non-serializable content.

## Related Projects

- **[devantler/homelab](https://github.com/devantler/homelab)** - My personal homelab setup, including an example of how I use KSail.

## Star History

[![Star History Chart](https://api.star-history.com/svg?repos=devantler/ksail&type=Date)](https://star-history.com/#devantler/ksail&Date)
