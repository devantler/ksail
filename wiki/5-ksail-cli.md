# KSail CLI

## `ksail`

```txt
Description:
  KSail is a CLI tool for provisioning GitOps enabled clusters in Docker.

Usage:
  ksail [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  check                 Check the status of a cluster
  debug                 Debug a cluster (❤️ K9s)
  down <clusterName>    Destroy a cluster
  init <clusterName>    Initialize a cluster
  lint <clusterName>    Lint manifests for a cluster
  list                  List active clusters
  sops <clusterName>    Manage secrets with SOPS
  start <clusterName>   Start a cluster
  stop <clusterName>    Stop a cluster
  up <clusterName>      Provision a cluster
  update <clusterName>  Update a cluster
```

## `ksail check`

```txt
Description:
  Check the status of a cluster

Usage:
  ksail check [options]

Options:
  -k, --kubeconfig <kubeconfig> (REQUIRED)  Path to kubeconfig file [default: /Users/nikolaiemildamm/.kube/config]
  -c, --context <context>                   The Kubernetes context to use
  -t, --timeout <timeout>                   The timeout in seconds to wait for each kustomization to become ready. Defaults to 600 seconds. [default:
                                            600]
  -?, -h, --help                            Show help and usage information
```

## `ksail debug`

```txt
Description:
  Debug a cluster (❤️ K9s)

Usage:
  ksail debug [options]

Options:
  -k, --kubeconfig <kubeconfig> (REQUIRED)  Path to kubeconfig file [default: /Users/nikolaiemildamm/.kube/config]
  -c, --context <context>                   The Kubernetes context to use
  -?, -h, --help                            Show help and usage information
```

## `ksail down <name-of-cluster>`

```txt
Description:
  Destroy a cluster

Usage:
  ksail down <clusterName> [options]

Arguments:
  <clusterName>

Options:
  -d, --delete-pull-through-registries  Delete pull through registries [default: False]
  -?, -h, --help                        Show help and usage information
```

## `ksail init <name-of-cluster>`

```txt
Description:
  Initialize a cluster

Usage:
  ksail init <clusterName> [options]

Arguments:
  <clusterName>

Options:
  -m, --manifests <manifests> (REQUIRED)  Path to the manifests directory [default: ./k8s]
  -?, -h, --help                          Show help and usage information
```

## `ksail lint <name-of-cluster>`

```txt
Description:
  Lint manifests for a cluster

Usage:
  ksail lint <clusterName> [options]

Arguments:
  <clusterName>

Options:
  -m, --manifests <manifests> (REQUIRED)  Path to the manifests directory [default: ./k8s]
  -?, -h, --help                          Show help and usage information
```

## `ksail list`

```txt
Description:
  List active clusters

Usage:
  ksail list [options]

Options:
  -?, -h, --help  Show help and usage information
```

## `ksail sops <name-of-cluster>`

```txt
Description:
  Manage secrets with SOPS

Usage:
  ksail sops <clusterName> [options]

Arguments:
  <clusterName>

Options:
  -g, --generate-key       Generate a new key
  --show-key               Show the full key
  --show-public-key        Show the public key
  --show-private-key       Show the private key
  -e, --encrypt <encrypt>  File to encrypt
  -d, --decrypt <decrypt>  File to decrypt
  --import <import>        Import a key
  --export <export>        Export a key
  -?, -h, --help           Show help and usage information
```

## `ksail start <name-of-cluster>`

```txt
Description:
  Start a cluster

Usage:
  ksail start <clusterName> [options]

Arguments:
  <clusterName>

Options:
  -?, -h, --help  Show help and usage information
```

## `ksail stop <name-of-cluster>`

```txt
Description:
  Stop a cluster

Usage:
  ksail stop <clusterName> [options]

Arguments:
  <clusterName>

Options:
  -?, -h, --help  Show help and usage information
```

## `ksail up <name-of-cluster>`

```txt
Description:
  Provision a cluster

Usage:
  ksail up <clusterName> [options]

Arguments:
  <clusterName>

Options:
  -c, --config <config> (REQUIRED)       Path to the cluster configuration file [default: k3d-config.yaml]
  -m, --manifests <manifests>            Path to the manifests directory [default: ./k8s]
  -k, --kustomizations <kustomizations>  Path to the flux kustomization directory [default: ./k8s/clusters/<clusterName>/flux-system]
  -t, --timeout <timeout>                The timeout in seconds to wait for each kustomization to become ready. Defaults to 600 seconds. [default: 600]
  -ns, --no-sops                         Disable SOPS [default: False]
  -sl, --skip-linting                    Skip linting of manifests [default: False]
  -?, -h, --help                         Show help and usage information
```

## `ksail update <name-of-cluster>`

```txt
Description:
  Update a cluster

Usage:
  ksail update <clusterName> [options]

Arguments:
  <clusterName>

Options:
  -m, --manifests <manifests> (REQUIRED)  Path to the manifests directory [default: ./k8s]
  -nl, --no-lint                          Skip linting manifests [default: False]
  -nr, --no-reconcile                     Skip reconciling manifests [default: False]
  -?, -h, --help                          Show help and usage information
```
