# Frequently Asked Questions

## Why use KSail instead of e.g. k3d or kind directly?

KSail is built on top of k3d, so it provides all the same functionality as k3d. However, KSail also provides additional functionality for managing GitOps-enabled Kubernetes clusters in Docker. For a GitOps-enabled cluster to work well in Docker, you need quite a few tools to be installed and configured. KSail aims to simplify this process by providing a set of commands that allow you to easily create, manage, and dismantle GitOps-enabled clusters.

### How can I run KSail as a Docker Container?

To run KSail as a Docker container you need to mount the Docker socket, your working directories, and KSail config files. You also need to run KSail on your host network to allow it to connect to containers on localhost.

```sh
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



### What is next for KSail?

Features in the pipeline:

- **KSail Gen:** With the template engine implemented I intend to add support for various generators that can generate anything from Kubernetes manifests to config files.
- **Kind Support:** KSail will be able to create and manage GitOps-enabled Kubernetes clusters in Kind.
- **Talos in Docker Support:** KSail will be able to create and manage GitOps-enabled Kubernetes clusters in Docker with Talos Linux.
- **Setting Hosts:** KSail will be able to set hosts for services made accessible through ingresses.
- **KSail YAML config:** As KSail matures, I will support more container engines and Kubernetes distributions, so a way to set defaults will be required. As such I plan to add support for a YAML config file to specify KSail-related settings and defaults. KSail will support generating the file if it does not exist, or generate it with `ksail gen` when the template engine matures.

Features I'm considering:

- **ArgoCD Support through Flamingo:** Working with YAML is not necessarily the preferred approach for all, so I am contemplating including Flamingo as a helm release provided by the `ksail init` command, so users can choose to create new releases from ArgoCDs proven UI. This might just be a matter of installing Flamingo, and configuring it to work with the structure KSail provides.
- **VCluster Support:** I am considering adding support for VCluster, so users can create and manage VClusters in existing clusters.
