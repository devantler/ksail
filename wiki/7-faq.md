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
