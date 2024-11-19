# Roadmap

The roadmap for KSail details the planned features and improvements for the project. The roadmap is not a exhaustive list of all the changes that will be made to KSail, but rather a high-level overview of the more prominent features and improvements that are planned for the project.

## 2024

- [x] Large refactor to extract all generic functionality into separate libraries. For example the template engine, the different APIs for embedded binaries, and implementations for e.g. provisioning and managing clusters.
- [x] KSail YAML Config to enable setting defaults for KSail that are overwritable by CLI flags. This will enable simplifying the CLI commands, such that users do not have to specify the same flags over and over again.
- [x] Bug fixes and small convenience features. For example there is a few issues around the hardcoded port range used for the OCI registry and pull-through caches, as well as some issues with the `ksail sops` command.

## 2025

- [x] KSail Gen to generate Kubernetes manifests, config files, and more. This will enable users to generate the files they need for their projects, without having to write them from scratch. This will be an iterative process, where initially only a few resources will be supported, and more will be added over time.
- [x] Kind support to enable users to create and manage GitOps-enabled Kubernetes clusters provisioned with Kind. This is important for users that prefer to use Kind over K3d.
- [ ] Talos in Docker support to enable users to create and manage GitOps-enabled Kubernetes clusters provisioned with Talos Linux in Docker. This is important for users that prefer to use Talos over K3d.

## Features I'm considering

- [ ] Setting hosts in the host file, to make it easier for users to make services accessible through ingresses.
- [ ] ArgoCD support through Flamingo, to enable users to manage their clusters through ArgoCDs UI. This is should be possible already, but it requires users to install Flamingo and deploy it to their clusters manually. It would be nice to have this option be available through the `ksail init` command.
- [ ] VCluster support so users can create and manage KSail clusters in a virtual cluster environment on existing Kubernetes clusters. This would be useful for scaling KSail clusters horizontally, as KSail clusters are currently limited to the resources available on the host machine, or the resources allocated to the Docker daemon. Furthermore this would make sure that KSail in CI scales well, whereas today it quickly reaches the limits of for example GitHub's hosted runners.
