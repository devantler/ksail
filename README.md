# ⛴️🐳 KSail

> [!NOTE]
> KSail and this README is currently a WIP. Please come back soon for a stable release 🚀

![image](https://github.com/devantler/ksail/assets/26203420/d8a20d3c-5152-40f8-a08d-2d70517e094d)


A CLI tool for provisioning GitOps enabled K8s environments in Docker.

## Getting Started

### Prerequisites

- Unix or Linux-based OS.
  - osx-x64 ✅
  - osx-arm64 ✅
  - linux-x64 ✅
  - linux-arm64 ✅
  - linux-arm ✅
  - windows-x86 ❌
  - windows-arm64 ❌
- Docker
- gpg

### Installation

With Homebrew:

```shell
TODO: Add a homebrew package for ksail.

brew install ksail
```

Manually:

1. Download the latest release from the [releases page](https://github.com/devantler/ksail/releases).
2. Make the binary executable: `chmod +x ksail`.
3. Move the binary to a directory in your `$PATH`: `mv ksail /usr/local/bin/ksail`.

### Usage

To get started use the global help flag:

```shell
ksail --help
```

## What is KSail?

Introducing KSail, your new go-to CLI tool for effortlessly managing GitOps-enabled Kubernetes clusters in Docker. Designed with simplicity and efficiency in mind, KSail is set to revolutionize your local Kubernetes development environment.

`ksail up` - Creating a Flux-enabled cluster has never been easier. With the up command, you can swiftly set up a cluster with your chosen Kubernetes-in-Docker backend. KSail empowers you to choose the backend that best fits your needs.

`ksail down` - Need to dismantle a Flux-enabled cluster? The down command has you covered. With a single command, you can easily tear down your cluster when it's no longer needed.

`ksail validate` - Ensure your cluster's manifest files are ready for deployment with the validate command. KSail simplifies the process of linting and validating manifest files, saving you valuable time and effort.

`ksail verify` - With the verify command, you can easily check that your cluster reconciles successfully. KSail streamlines the verification process, making it easier than ever to ensure your cluster is functioning as expected.

`ksail sops` - KSail makes it easy to manage secrets in Git repositories. With the sops command, you can quickly create, get, and share your KSail SOPS GPG key. KSail takes the hassle out of managing secrets, allowing you to focus on what matters most.

KSail is more than just a tool - it's your partner in navigating the world of Kubernetes. Get ready to set sail with KSail!

## How does it work?

KSail leverages several key technologies and concepts to achieve its functionality:

**Embedded binaries:** KSail embeds binaries for various tools like k3d, talosctl, and sops. This allows you to use these tools without having to install them on your system. The binaries use state-of-the-art compression techniques by Microsoft to ensure only the necessary files and code is included in the build.

**Kubernetes-in-Docker Backends:** KSail supports various Kubernetes-in-Docker backends like k3d and talos. These backends allow you to run Kubernetes clusters inside Docker containers, which is ideal for local development and testing.

**Flux GitOps:** KSail uses Flux, a GitOps tool for Kubernetes, to manage the state of your clusters. With GitOps, your manifest source serves as the single source of truth for your cluster's desired state. Any changes to the source trigger an update in your cluster.

**Local OCI Registries:** KSail uses local Open Container Initiative (OCI) registries to store and distribute Docker images and manifests. When you push manifest files to these registries, it triggers updates in your Kubernetes clusters. This allows for a streamlined and efficient workflow for updating your applications.

**SOPS Integration:** KSail integrates with SOPS, a tool for managing secrets in Git repositories. It does so by automatically creating a SOPS GPG key and configuring it to work with your KSail clusters. This allows you to easily encrypt and decrypt your secrets.

**Manifest Validation:** Before deploying your clusters, KSail validates your manifest files to ensure they are correctly formatted and contain valid configurations. This helps catch errors before they cause problems in your clusters.

**Cluster Reconciliation Verification:** After deploying your clusters, KSail verifies that they reconcile successfully. This means it checks that the actual state of your clusters matches the desired state defined in your configuration files.

**Flags and Options:** KSail provides a variety of flags and options for its commands, allowing you to customize its behavior to suit your needs. This makes KSail flexible enough to support a wide range of workflows.

In summary, KSail is a powerful tool that combines several technologies and practices to make it easier to work with Kubernetes in Docker, particularly in a GitOps context.

## Why was it made?

The journey to create KSail was sparked by a noticeable gap in the tooling landscape for managing GitOps-enabled Kubernetes clusters in Docker. While there were tools available for individual tasks, there was a clear need for a comprehensive solution that could handle the entire process in a streamlined and efficient manner.

KSail was born out of the desire to simplify the process of bootstrapping Flux, setting up OCI registries, managing SOPS, and pushing manifest files to local OCI registries to trigger updates. But that's just the beginning! The vision for KSail extends far beyond these initial features.

Future plans for KSail include the ability to achieve instant reconciliation by preconfiguring clusters with Flux webhook receivers. These receivers can be triggered by KSail itself, allowing for immediate reconciliations and providing users with instant feedback on the state of their clusters.

In essence, KSail was created to bring simplicity and efficiency to managing GitOps workflows in local Kubernetes development environments. It's more than just a tool - it's a solution designed to make your Kubernetes journey smoother and more enjoyable.

## Contributing

I welcome contributions to KSail and appreciate your help in improving this tool. Here's how you can contribute:

**Reporting Bugs or Requesting Features:** If you encounter any bugs while using KSail, or if you have a feature request, please create an issue in the GitHub repository. When creating an issue, provide as much detail as possible to help me understand the problem or feature.

**Submitting Pull Requests:** If you have a fix for a bug, an improvement, or a new feature you'd like to add, you're welcome to create a pull request. However, because not all changes may align with the project's direction, I recommend starting a discussion in the issues section before working on a pull request. This can save both you and me time and ensure that your contributions can be integrated smoothly.

Remember, your contributions, no matter how small, are greatly appreciated and help make KSail better for everyone. Thank you for your support!
