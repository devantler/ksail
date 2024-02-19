# 🛥️🐳 KSail

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Test](https://github.com/devantler/ksail/actions/workflows/test.yaml/badge.svg?branch=main)](https://github.com/devantler/ksail/actions/workflows/test.yaml)
[![codecov](https://codecov.io/gh/devantler/ksail/graph/badge.svg?token=DNEO90PfNR)](https://codecov.io/gh/devantler/ksail)

> [!NOTE]
> This is an early release of KSail. I am actively working on the tool, so if you encounter any issues, please let me know 🙏🏻

![image](https://github.com/devantler/ksail/assets/26203420/8ec4e51c-e85a-48c2-9b82-4b7bb0de1167)

<details>
  <summary>Show/hide folder structure</summary>

<!-- readme-tree start -->
```
.
├── .github
│   └── workflows
├── .vscode
├── autocomplete
├── scripts
├── src
│   └── KSail
│       ├── Arguments
│       ├── CLIWrappers
│       ├── Commands
│       │   ├── Check
│       │   │   ├── Handlers
│       │   │   └── Options
│       │   ├── Down
│       │   │   ├── Handlers
│       │   │   └── Options
│       │   ├── Init
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
│       ├── Options
│       ├── Provisioners
│       │   ├── ContainerEngine
│       │   ├── ContainerOrchestrator
│       │   ├── GitOps
│       │   ├── KubernetesDistribution
│       │   └── SecretManager
│       └── assets
│           ├── binaries
│           └── k3d
└── tests
    └── KSail.Tests.Integration
        ├── Commands
        │   ├── Check
        │   ├── Down
        │   ├── Lint
        │   ├── List
        │   ├── Root
        │   ├── SOPS
        │   ├── Up
        │   └── Update
        └── TestUtils

61 directories
```
<!-- readme-tree end -->

</details>

## Getting Started

### Prerequisites

Supported OSes:

- darwin-amd64 🍎✅
- darwin-arm64 🍎✅
- linux-amd64 🐧✅
- linux-arm64 🐧✅
- windows-amd64 🪟❌
- windows-arm64 🪟❌

Tools:

- [Docker](https://www.docker.com) (required)

### Recommendations

Tools:

- [K9s](https://k9scli.io) (for debugging)
- [VScode Extension - Run on Save(pucelle.run-on-save)](https://github.com/pucelle/vscode-run-on-save) (run `ksail update` on save, to enable "live updates")
- [VSCode Extension - GitOps Tools for Flux](https://marketplace.visualstudio.com/items?itemName=Weaveworks.vscode-gitops-tools) (UI to watch and debug reconciliations)

### Installation

With Homebrew:

```sh
brew tap devantler/formulas
brew install ksail
```

Manually:

1. Download the latest release from the [releases page](https://github.com/devantler/ksail/releases).
2. Make the binary executable: `chmod +x ksail`.
3. Move the binary to a directory in your `$PATH`: `mv ksail /usr/local/bin/ksail`.

### Usage

KSail is built to run as either a local binary, or as a Docker container.

Setting sail for your voyage and navigating beyond the shore with KSail is as straightforward as:

```sh
# --- Local Binary ---
ksail init <name-of-cluster>
ksail up <name-of-cluster>

# --- Docker Container ---
docker run --rm \
  -v $(pwd):/app `# Mount working directories` \
  ghcr.io/devantler/ksail:latest init <name-of-cluster>

docker run --rm \
  -v /var/run/docker.sock:/var/run/docker.sock `# Mount Docker socket` \
  -v $(pwd):/app `# Mount working directories` \
  -v $(pwd):/root/.ksail `# Mount KSail config files` \
  --network host `# Allow access to containers on localhost` \
  ghcr.io/devantler/ksail:latest up <name-of-cluster>
```

For more intricate navigational techniques, consult the global --help flag:

```sh
# --- Local Binary ---
ksail --help

# --- Docker Container ---
docker run --rm ghcr.io/devantler/ksail:latest --help
```

### Supported Environment Variables

| Variable Name    | Description                                                | Default Value |
| :--------------- | ---------------------------------------------------------- | :-----------: |
| `KSAIL_SOPS_KEY` | The SOPS key to use for encrypting and decrypting secrets. |     `""`      |

## What is KSail?

KSail is a CLI tool designed to simplify the management of GitOps-enabled Kubernetes clusters in Docker. It provides a set of commands that allow you to easily create, manage, and dismantle GitOps-enabled clusters. KSail also integrates with SOPS for managing secrets in Git repositories and provides features for validating and verifying your clusters.

KSail provides the following features:

- **Initialize YAML and configuration:** KSail can be used to generate needed YAML and configuration files for your clusters.
- **Create clusters:** KSail can be used to create GitOps-enabled Kubernetes clusters in Docker.
- **Sync clusters:** KSail can be used to sync GitOps-enabled Kubernetes clusters in Docker (both manually and automatically).
- **Lint manifests:** KSail can be used to lint your manifest files before deploying your clusters.
- **Check cluster reconciliations:** KSail can be used to verify that your clusters reconcile successfully after deployment.
- **Manage secrets:** KSail can be used to manage secrets in Git repositories.
- **Docker Container Support:** KSail can be run as a Docker container.

## How does it work?

KSail leverages several key technologies to provide its functionality:

- **Embedded Binaries:** KSail embeds binaries for tools like k3d, flux, age, and sops. This enables KSail to work out of the box without requiring you to install any additional dependencies.
- **K3d Backend:** KSail uses K3d, allowing you to run Kubernetes clusters inside Docker containers with a small footprint.
- **Flux GitOps:** KSail sets up Flux GitOps to manage the state of your clusters, with your manifest source serving as the single source of truth.
- **Local OCI Registries:** KSail uses local OCI registries to store and distribute Docker images and manifests.
- **SOPS and Age Integration:** KSail integrates with SOPS and Age for managing secrets in Git repositories.
- **Kustomize and Kubeconform Integration:** KSail integrates with Kustomize and Kubeconform for linting your manifest files before deploying your clusters.
- **Kubernetes API:** KSail uses the Kubernetes API to verify that your clusters reconcile successfully after deployment.

## Why was it made?

KSail was created to fill a gap in the tooling landscape for managing GitOps-enabled Kubernetes clusters in Docker. There are currently two intended use cases for KSail:

- **Local Development:** KSail can be used to create and manage GitOps-enabled Kubernetes clusters in Docker for local development. This allows you to easily build and test your applications in a K8s environment.
- **CI/CD:** KSail can be used to spin up GitOps-enabled Kubernetes clusters in CI/CD, to easily verify that your changes are working as expected before deploying them to your other environments.

## Q&A

### Why use KSail instead of e.g. k3d or kind?

KSail is built on top of k3d, so it provides all the same functionality as k3d. However, KSail also provides additional functionality for managing GitOps-enabled Kubernetes clusters in Docker. For a GitOps-enabled cluster to work well in Docker, you need quite a few tools to be installed and configured. KSail aims to simplify this process by providing a set of commands that allow you to easily create, manage, and dismantle GitOps-enabled clusters.

### How do I use KSail with CI/CD?

You need to download the KSail binary into your CI/CD environment, and then run the KSail commands as you would locally. For example, if you are using GitHub Actions, you can use the following workflow:

```yaml
name: KSail

on:
  pull_request:
    branches: [main]
  push:
    branches: [main]

concurrency:
  group: ${{ github.workflow }}-${{ github.ref }}
  cancel-in-progress: true

jobs:
  ksail:
    runs-on: ubuntu-latest
    steps:
      - name: 📑 Checkout
        uses: actions/checkout@v4
      - name: 🍺 Set up Homebrew
        uses: Homebrew/actions/setup-homebrew@master
      - name: 🛥️🐳 Install KSail
        run: brew install devantler/formulas/ksail
      - name: 🛥️🐳🚀 KSail Up
        run: |
          ksail sops <name-of-cluster> --import "${{ secrets.KSAIL_SOPS_AGE_KEY }}"
          ksail up <name-of-cluster>
```

### How do I use KSail with Cloud Providers?

KSail is purposely designed to work with local Docker clusters. If you want to create clusters in the cloud, I recommend using an Infrastructure as Code (IaC) tool like [Terraform](https://github.com/hashicorp/terraform) or [Pulumi](https://github.com/pulumi/pulumi) to create your clusters and initialize Flux GitOps. You can still use KSail to generate the needed YAML and configuration files, but clusters in the cloud often require additional configuration and dependencies, so do not expect your Docker clusters to work in the cloud without some additional work.

### What is next for KSail?

I am currently working on stabilizing the tool, and ensure that it works as expected, and if not that it fails gracefully with informative error messages. I am also working on adding more tests, and improving the test coverage. Once I am happy with the stability of the tool, I will start working on adding more features.

Features in the pipeline:

- **100% Test Coverage:** KSail is currently at ~80% test coverage, and I am working on getting it to 100%, to ensure that all intended use cases are thoroughly tested.
- **Better Error Handling:** KSail currently has some issues with error handling, and I am working on improving this, so that it fails gracefully with informative error messages.
- **Extra Args**: I intend to add support for passing extra arguments to the different commands, so users can choose to pass extra arguments to the underlying binaries if they so desire. If one command targets multiple binaries, e.g. `ksail up`, I intend to add support for passing extra arguments to the different binaries, e.g. `ksail up --flux-args="--some-arg" --k3d-args="--some-other-arg"`.
- **Improved Init Command:** I intend to build a small template engine into KSail, so it is easier to extend and customize the generated files.
- **Kind Support:** KSail will be able to create and manage GitOps-enabled Kubernetes clusters in Kind.

Features I'm considering:

- **ArgoCD Support through Flamingo:** Working with YAML is a not necessarily the preffered approach for all, so I am contemplating including Flaming as a helm release provided by the `ksail init` command, so users can choose to create new releases from ArgoCDs proven UI.
- **Windows Support:** Ideally, KSail should work on all platforms, but the current setup has a few hindrances that make it difficult to support Windows. I am contemplating how to best solve this, or if I should just drop Windows support altogether.
- **Setting hosts:** Services made accessible through ingresses cannot be reached without setting their dns in the hosts file. I believe it would be nice if KSail was able to do this in a friendly way. EDIT: On second thought I do not believe this feature will be very flexible, so instead I am to document how this can be done manually, but as many might have different setups, or even local DNS servers, I do not believe this is a good fit for KSail.

## Contributing

Contributions to KSail are welcome! You can contribute by reporting bugs, requesting features, or submitting pull requests. When creating an issue or pull request, please provide as much detail as possible to help understand the problem or feature. Check out the [Contribution Guidelines](https://github.com/devantler/ksail/blob/main/CONTRIBUTING.md) for more info.
