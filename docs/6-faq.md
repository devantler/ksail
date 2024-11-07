# Frequently Asked Questions

## Why use KSail, when I can just use the tools it is built on top of?

KSail was created to provide a better Developer Experience (DX) than having to juggle a bunch of different CLI tools to even get started with Kubernetes. The goal is to lower the barrier to entry for working with Kubernetes, and to provide a more streamlined experience for managing GitOps-enabled Kubernetes clusters that you can build locally and deploy to your remote environments.

So in short, you do not have to use KSail, but I highly recommend it if you want the simplicity that it provides. And if find that it is missing something that you need, please reach out and let me know. I want this tool to make it easier for everyone, not only those that have simple use cases.

### How can I run KSail as a Docker Container?

To run KSail as a Docker container you need to mount the Docker socket, your working directories, and KSail config files. You also need to run KSail on your host network to allow it to connect to containers on localhost.

```sh
docker run --rm \
  -v $(pwd):/app `# Mount working directories` \
  ghcr.io/devantler/ksail:latest init

docker run --rm \
  -v /var/run/docker.sock:/var/run/docker.sock `# Mount Docker socket` \
  -v $(pwd):/app `# Mount working directories` \
  -v $(pwd):/root/.ksail `# Mount KSail config files` \
  --network host `# Allow access to containers on localhost` \
  ghcr.io/devantler/ksail:latest up
```
