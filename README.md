# KSail

> [!NOTE]
> KSail and this README is currently a WIP. Please come back soon for a stable release 🚀

```
                                     . . .
                __/___                 :
          _____/______|             ___|____     |"\/"|
  _______/_____\_______\_____     ,'        `.    \  /
  \               KSail      |    |  ^        \___/  |
~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~
```

A kubernetes-in-docker service for provisioning GitOps enabled K8s environments in Docker.

## Getting Started

### Prerequisites

- A Unix or Linux based OS.
- A Flux project.

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
ksail -h
```

## What is KSail?

Introducing KSail, your new go-to CLI tool for effortlessly managing GitOps-enabled Kubernetes clusters in Docker. Designed with simplicity and efficiency in mind, KSail is set to revolutionize your local Kubernetes development environment.

`ksail install` - Say goodbye to the hassle of managing dependencies for Kubernetes-in-Docker backends. KSail's install command serves as a one-stop solution, seamlessly handling installation and updates for backends like k3d, talos, and minikube.

`ksail up` - Creating a Flux-enabled cluster has never been easier. With the up command, you can swiftly set up a cluster with your chosen Kubernetes-in-Docker backend. KSail empowers you to choose the backend that best fits your needs.

`ksail down` - Need to dismantle a Flux-enabled cluster? The down command has you covered. With a single command, you can easily tear down your cluster when it's no longer needed.

`ksail validate` - Ensure your cluster's manifest files are ready for deployment with the validate command. KSail simplifies the process of linting and validating manifest files, saving you valuable time and effort.

`ksail install` - With the verify command, you can easily check that your cluster reconciles successfully. KSail streamlines the verification process, making it easier than ever to ensure your cluster is functioning as expected.

KSail is more than just a tool - it's your partner in navigating the world of Kubernetes. Get ready to set sail with KSail!

## How does it work?

KSail leverages several key technologies and concepts to achieve its functionality:

**Kubernetes-in-Docker Backends:** KSail supports various Kubernetes-in-Docker backends like k3d and talos. These backends allow you to run Kubernetes clusters inside Docker containers, which is ideal for local development and testing.

**Flux GitOps:** KSail uses Flux, a GitOps tool for Kubernetes, to manage the state of your clusters. With GitOps, your manifest source serves as the single source of truth for your cluster's desired state. Any changes to the source trigger an update in your cluster.

**Local OCI Registries:** KSail uses local Open Container Initiative (OCI) registries to store and distribute Docker images and manifests. When you push manifest files to these registries, it triggers updates in your Kubernetes clusters. This allows for a streamlined and efficient workflow for updating your applications.

**Manifest Validation:** Before deploying your clusters, KSail validates your manifest files to ensure they are correctly formatted and contain valid configurations. This helps catch errors before they cause problems in your clusters.

**Cluster Reconciliation Verification:** After deploying your clusters, KSail verifies that they reconcile successfully. This means it checks that the actual state of your clusters matches the desired state defined in your configuration files.

**Flags and Options:** KSail provides a variety of flags and options for its commands, allowing you to customize its behavior to suit your needs. This makes KSail flexible enough to support a wide range of workflows.

In summary, KSail is a powerful tool that combines several technologies and practices to make it easier to work with Kubernetes in Docker, particularly in a GitOps context.

## Why was it made?

The journey to create KSail was sparked by a noticeable gap in the tooling landscape for managing GitOps-enabled Kubernetes clusters in Docker. While there were tools available for individual tasks, there was a clear need for a comprehensive solution that could handle the entire process in a streamlined and efficient manner.

KSail was born out of the desire to simplify the process of bootstrapping Flux, setting up OCI registries, managing SOPS, and pushing manifest files to local OCI registries to trigger updates. But that's just the beginning! The vision for KSail extends far beyond these initial features.

Future plans for KSail include the ability to achieve instant reconciliation by preconfiguring clusters with Flux webhook receivers. These receivers can be triggered by KSail itself, allowing for immediate reconciliations and providing users with instant feedback on the state of their clusters.

In essence, KSail was created to bring simplicity and efficiency to managing GitOps workflows in local Kubernetes development environments. It's more than just a tool - it's a solution designed to make your Kubernetes journey smoother and more enjoyable.
