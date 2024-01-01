#!/bin/bash
function main() {
  function check_os() {
    if [[ "$OSTYPE" != "darwin"* && "$OSTYPE" != "linux-gnu"* ]]; then
      echo "🚫 Unsupported OS. KSail only supports Unix and Linux based operating systems. Exiting..."
      exit 1
    fi
  }

  function define_colors() {
    RED='\033[1;31m'
    GREEN='\033[1;32m'
    YELLOW='\033[1;33m'
    BLUE='\033[1;34m'
    PURPLE='\033[1;35m'
    WHITE='\033[0m'
  }

  function define_font_types() {
    NORMAL=$(tput sgr0)
    BOLD=$(tput bold)
    ITALIC=$(tput sitm)
    UNDERLINE=$(tput smul)
  }

  function help() {
    function help_no_arg() {
      echo "Usage:"
      echo "  ksail [COMMAND] [FLAGS]"
      echo
      echo "Commands:"
      echo "  install    install dependencies"
      echo "  up         create cluster"
      echo "  down       destroy cluster"
      echo "  validate   validate cluster manifest files"
      echo "  verify     verify cluster reconciliation"
      echo
      echo "Global Flags:"
      echo "  -h, --help      print help information"
      echo "  -v, --version   print version information"
    }

    function help_up_arg() {
      echo "Usage:"
      echo "  ksail up [FLAGS]"
      echo
      echo "Flags:"
      echo -e "  -n, --name      name of the cluster (${GREEN}ksail${WHITE})"
      echo -e "  -b, --backend   k8s-in-docker backend (${GREEN}k3d${WHITE}, talos)"
      echo -e "  -p, --path      path to the flux kustomization manifests (${GREEN}./${WHITE})"
      echo
      echo "ℹ️ Info:"
      echo "- If the k3d backend is used, all docker resources will be prefixed with 'k3d-'. This is a naming pattern that k3d uses for a variety of its features."
      echo
      echo "⚠️ Warnings:"
      echo -e "- The clusters created by KSail are not meant for production use."
    }

    function help_down_arg() {
      echo "Usage:"
      echo "  ksail down [FLAGS]"
      echo
      echo "Flags:"
      echo -e "  -n, --name      name of the cluster (${GREEN}ksail${WHITE})"
      echo -e "  -b, --backend   k8s-in-docker backend (k3d, ${GREEN}talos${WHITE})"
    }

    function help_validate_arg() {
      echo "Usage:"
      echo "  ksail validate [FLAGS]"
      echo
      echo "Flags:"
      echo "  -h, --help  # Print help information"
    }

    function help_verify_arg() {
      echo "Usage:"
      echo "  ksail verify [FLAGS]"
      echo
      echo "Flags:"
      echo "  -h, --help  # Print help information"
    }

    if [ -z "${1}" ]; then
      help_no_arg
    else
      while [ $# -gt 0 ]; do
        case "$1" in
        up)
          help_up_arg "$@"
          exit
          ;;
        down)
          help_down_arg "$@"
          exit
          ;;
        validate)
          help_validate_arg "$@"
          exit
          ;;
        verify)
          help_verify_arg "$@"
          exit
          ;;
        *)
          echo "Unknown argument: $1"
          exit 1
          ;;
        esac
      done
    fi
  }

  function run() {
    function version() {
      echo "KSail 0.0.1"
    }

    function run_no_arg() {
      function introduction() {
        echo -e "⛴️ 🐳   ${BOLD}${UNDERLINE}Welcome to ${BLUE}KSail${WHITE}   ⛴️ 🐳${NORMAL}"
        echo "                                     . . ."
        echo "                __/___                 ."
        echo '          _____/______|             ___:____     |"\/"|'
        echo "  _______/_____\_______\_____     ,'        \`.    \  /"
        echo "  \    k8s       < < <       |    |  ^        \___/  |"
        echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~^~^~^~^~^~^~^~^~^~^~^~^~"
        echo
        echo -e "${BLUE}KSail${WHITE} can help you provision ${GREEN}GitOps enabled K8s environments${WHITE} in ${BLUE}Docker${WHITE}."
        echo
      }
      introduction
      help
    }

    function run_install() {
      echo "📦 Installing dependencies"
      if command -v brew &>/dev/null; then
        echo "📦✅ Homebrew already installed. Updating..."
        brew upgrade
      else
        echo "📦🔨 Installing Homebrew"
        /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
        (
          echo
          echo 'eval "$(/home/linuxbrew/.linuxbrew/bin/brew shellenv)"'
        ) >>/home/runner/.bashrc
        eval "$(/home/linuxbrew/.linuxbrew/bin/brew shellenv)"
        echo "📦✅ Homebrew installed"
      fi

      if command -v yq &>/dev/null; then
        echo "📦✅ YQ already installed. Skipping..."
      else
        echo "📦🔨 Installing YQ"
        brew install yq
        echo "📦✅ YQ installed"
      fi

      if command -v kubeconform &>/dev/null; then
        echo "📦✅ Kubeconform already installed. Skipping..."
      else
        echo "📦🔨 Installing Kubeconform"
        brew install kubeconform
        echo "📦✅ Kubeconform installed"
      fi

      if command -v kustomize &>/dev/null; then
        echo "📦✅ Kustomize already installed. Skipping..."
      else
        echo "📦🔨 Installing Kustomize"
        brew install kustomize
        echo "📦✅ Kustomize installed"
      fi

      if command -v docker &>/dev/null; then
        echo "📦✅ Docker already installed. Skipping..."
      else
        echo "📦🔨 Installing Docker"
        brew install --cask docker
        echo "📦✅ Docker installed"
      fi

      if command -v talosctl &>/dev/null; then
        echo "📦✅ Talosctl already installed. Skipping..."
      else
        echo "📦🔨 Installing Talosctl"
        brew install siderolabs/talos/talosctl
        echo "📦✅ Talosctl installed"
      fi

      if command -v flux &>/dev/null; then
        echo "📦✅ Flux already installed. Skipping..."
      else
        echo "📦🔨 Installing Flux"
        brew install fluxcd/tap/flux
        echo "📦✅ Flux installed"
      fi

      if command -v gpg &>/dev/null; then
        echo "📦✅ GPG already installed. Skipping..."
      else
        echo "📦🔨 Installing GPG"
        brew install gpg
        echo "📦✅ GPG installed"
      fi

      if command -v kubectl &>/dev/null; then
        echo "📦✅ Kubectl already installed. Skipping..."
      else
        echo "📦🔨 Installing Kubectl"
        brew install kubectl
        echo "📦✅ Kubectl installed"
      fi
    }

    function run_up() {
      function check_if_docker_is_running() {
        echo "🐳 Checking if Docker is running"
        if docker info &>/dev/null; then
          echo "🐳✅ Docker is running"
        else
          echo "🐳🚨 Docker is not running. Exiting..."
          exit 1
        fi
        echo
      }

      function create_oci_registries() {
        function check_registry_exists() {
          local registry_name=${1}
          if (docker volume ls | grep -q "${registry_name}") && (docker container ls -a | grep -q "${registry_name}"); then
            true
          else
            false
          fi
        }
        echo "🧮 Adding pull-through registries"
        if check_registry_exists proxy-docker.io; then
          echo "🧮✅ Registry 'proxy-docker.io' already exists. Skipping..."
        else
          echo "🧮🔨 Creating registry 'proxy-docker.io'"
          docker run -d -p 5001:5000 \
            -e REGISTRY_PROXY_REMOTEURL=https://registry-1.docker.io \
            --restart always \
            --name proxy-docker.io \
            --volume proxy-docker.io:/var/lib/registry \
            registry:2 || {
            echo "🚨 Registry creation failed. Exiting..."
            exit 1
          }
        fi

        if check_registry_exists proxy-docker-hub.com; then
          echo "🧮✅ Registry 'proxy-docker-hub.com' already exists. Skipping..."
        else
          echo "🧮🔨 Creating registry 'proxy-docker-hub.com'"
          docker run -d -p 5002:5000 \
            -e REGISTRY_PROXY_REMOTEURL=https://hub.docker.com \
            --restart always \
            --name proxy-docker-hub.com \
            --volume proxy-docker-hub.com:/var/lib/registry \
            registry:2 || {
            echo "🚨 Registry creation failed. Exiting..."
            exit 1
          }
        fi

        if check_registry_exists proxy-registry.k8s.io; then
          echo "🧮✅ Registry 'proxy-registry.k8s.io' already exists. Skipping..."
        else
          echo "🧮🔨 Creating registry 'proxy-registry.k8s.io'"
          docker run -d -p 5003:5000 \
            -e REGISTRY_PROXY_REMOTEURL=https://registry.k8s.io \
            --restart always \
            --name proxy-registry.k8s.io \
            --volume proxy-registry.k8s.io:/var/lib/registry \
            registry:2 || {
            echo "🚨 Registry creation failed. Exiting..."
            exit 1
          }
        fi

        if check_registry_exists proxy-gcr.io; then
          echo "🧮✅ Registry 'proxy-gcr.io' already exists. Skipping..."
        else
          echo "🧮🔨 Creating registry 'proxy-gcr.io'"
          docker run -d -p 5004:5000 \
            -e REGISTRY_PROXY_REMOTEURL=https://gcr.io \
            --restart always \
            --name proxy-gcr.io \
            --volume proxy-gcr.io:/var/lib/registry \
            registry:2 || {
            echo "🚨 Registry creation failed. Exiting..."
            exit 1
          }
        fi

        if check_registry_exists proxy-ghcr.io; then
          echo "🧮✅ Registry 'proxy-ghcr.io' already exists. Skipping..."
        else
          echo "🧮🔨 Creating registry 'proxy-ghcr.io'"
          docker run -d -p 5005:5000 \
            -e REGISTRY_PROXY_REMOTEURL=https://ghcr.io \
            --restart always \
            --name proxy-ghcr.io \
            --volume proxy-ghcr.io:/var/lib/registry \
            registry:2 || {
            echo "🚨 Registry creation failed. Exiting..."
            exit 1
          }
        fi

        if check_registry_exists proxy-quay.io; then
          echo "🧮✅ Registry 'proxy-quay.io' already exists. Skipping..."
        else
          echo "🧮🔨 Creating registry 'proxy-quay.io'"
          docker run -d -p 5006:5000 \
            -e REGISTRY_PROXY_REMOTEURL=https://quay.io \
            --restart always \
            --name proxy-quay.io \
            --volume proxy-quay.io:/var/lib/registry \
            registry:2 || {
            echo "🚨 Registry creation failed. Exiting..."
            exit 1
          }
        fi

        if check_registry_exists manifests; then
          echo "🧮✅ Registry 'manifests' already exists. Skipping..."
        else
          echo "🧮🔨 Creating registry 'manifests'"
          docker run -d -p 5050:5000 \
            --restart always \
            --name manifests \
            --volume manifests:/var/lib/registry \
            registry:2 || {
            echo "🚨 Registry creation failed. Exiting..."
            exit 1
          }
        fi
        echo
      }

      function provision_cluster() {
        function add_sops_gpg_key() {
          echo "🔐 Adding SOPS GPG key"
          kubectl create namespace flux-system
          if [[ -z ${KSAIL_SOPS_GPG_KEY} ]]; then
            # TODO: Create new SOPS GPG key and set it to the KSAIL_SOPS_GPG_KEY variable
            gpg --batch --passphrase '' --quick-gen-key ksail default default
            local fingerprint
            fingerprint=$(gpg --list-keys -uid ksail | grep '^      *' | tr -d ' ')
            export KSAIL_SOPS_GPG_KEY
            KSAIL_SOPS_GPG_KEY=$(gpg--export-secret-keys --armor "$fingerprint")
          else
            kubectl create secret generic sops-gpg \
              --namespace=flux-system \
              --from-literal=sops.asc="${KSAIL_SOPS_GPG_KEY}" ||
              {
                echo "🚨 SOPS GPG key creation failed. Exiting..."
                exit 1
              }
          fi
        }

        function install_flux() {
          local cluster_name=${1}
          local docker_gateway_ip=${2}
          echo "🚀 Installing Flux"
          flux check --pre || {
            echo "🚨 Flux prerequisites check failed. Exiting..."
            exit 1
          }
          flux install || {
            echo "🚨 Flux installation failed. Exiting..."
            exit 1
          }
          local source_url="oci://$docker_gateway_ip:5050/${cluster_name}"
          flux create source oci flux-system \
            --url="$source_url" \
            --insecure=true \
            --tag=latest || {
            echo "🚨 Flux OCI source creation failed. Exiting..."
            exit 1
          }

          flux create source oci devantler-manifests \
            --url=oci://ghcr.io/devantler/oci-registry/manifests \
            --tag=latest || {
            echo "🚨 Flux OCI source creation failed. Exiting..."
            exit 1
          }

          flux create kustomization flux-system \
            --source=OCIRepository/flux-system \
            --path=./clusters/docker/flux || {
            echo "🚨 Flux kustomization creation failed. Exiting..."
            exit 1
          }
        }

        function provision_k3d_cluster() {
          echo
        }

        function provision_talos_cluster() {
          local cluster_name=${1}
          local docker_gateway_ip
          docker_gateway_ip=$(docker network inspect bridge --format='{{(index .IPAM.Config 0).Gateway}}')
          if [[ "$OSTYPE" == "darwin"* ]]; then
            docker_gateway_ip="192.168.65.254"
          fi
          echo "⛴️ Provision ${cluster_name} cluster"
          talosctl cluster create \
            --name "${cluster_name}" \
            --registry-mirror docker.io=http://"$docker_gateway_ip":5001 \
            --registry-mirror hub.docker.com=http://"$docker_gateway_ip":5002 \
            --registry-mirror registry.k8s.io=http://"$docker_gateway_ip":5003 \
            --registry-mirror gcr.io=http://"$docker_gateway_ip":5004 \
            --registry-mirror ghcr.io=http://"$docker_gateway_ip":5005 \
            --registry-mirror quay.io=http://"$docker_gateway_ip":5006 \
            --registry-mirror manifests=http://"$docker_gateway_ip":5050 \
            --wait || {
            echo "🚨 Cluster creation failed. Exiting..."
            exit 1
          }
          # talosctl config nodes 10.5.0.2 10.5.0.3 || {
          #   echo "🚨 Cluster configuration failed. Exiting..."
          #   exit 1
          # }

          # TODO: Add support for Talos patching
        }

        local cluster_name=${1}
        local backend=${2}
        local path=${3}

        if [[ "$backend" == "k3d" ]]; then
          provision_k3d_cluster "$cluster_name"
        elif [[ "$backend" == "talos" ]]; then
          provision_talos_cluster "$cluster_name" "$path"
        else
          echo "🚫 Unsupported backend. Exiting..."
          exit 1
        fi
        add_sops_gpg_key || {
          echo "🚨 SOPS GPG key creation failed. Exiting..."
          exit 1
        }
        install_flux "$cluster_name" "$docker_gateway_ip" || {
          echo "🚨 Flux installation failed. Exiting..."
          exit 1
        }
        echo
      }

      local cluster_name
      local backend
      local path
      if [ -z "$2" ]; then
        echo -e "${BOLD}What would you like to name your cluster? (default: ${GREEN}ksail${WHITE})${NORMAL}"
        read -r cluster_name
        if [ -z "$cluster_name" ]; then
          cluster_name="ksail"
        fi
        echo

        # Select backend using arrow keys. Default is k3d.
        echo -e "${BOLD}What backend would you like to use?${NORMAL}"
        PS3="Your selection: "
        options=("k3d" "talos")
        select opt in "${options[@]}"; do
          case $opt in
          "k3d")
            backend="k3d"
            break
            ;;
          "talos")
            backend="talos"
            break
            ;;
          *)
            echo "🚫 Invalid option: $REPLY."
            echo "   You must type the number of the option you want to select."
            echo
            ;;
          esac
        done

        echo -e "${BOLD}What is the path to your flux kustomization manifests? (default: ${GREEN}./${WHITE})${NORMAL}"
        read -r path
        if [ -z "$path" ]; then
          path="./"
        fi
        echo
      else
        local OPTIND=2
        while getopts ":hn:b:p:" flag; do
          case "${flag}" in
          h)
            help up
            exit
            ;;
          n)
            cluster_name=${OPTARG}
            ;;
          b)
            backend=${OPTARG}
            ;;
          p)
            path=${OPTARG}
            shift
            ;;
          *)
            echo "🚫 Unknown flag: $2"
            exit 1
            ;;
          esac
        done
        if [[ "$2" != "-"* ]]; then
          echo "🚫 Unknown flag: $2"
          exit 1
        fi
      fi

      check_if_docker_is_running
      create_oci_registries
      provision_cluster "$cluster_name" "$backend" "$path"
    }

    function run_down() {
      echo "down"
    }

    function run_validate() {
      echo "validate"
    }

    function run_verify() {
      echo "verify"
    }

    function run_args() {
      if [[ "$1" != "-"* ]]; then
        echo "🚫 Unknown flag: $1"
        exit 1
      fi
      while getopts ":hv" flag; do
        case "${flag}" in
        h)
          help
          ;;
        v)
          version "0.0.1"
          ;;
        \?)
          echo "🚫 Unknown flag: $1"
          exit 1
          ;;
        *)
          shift
          ;;
        esac
      done
    }

    if [ $# -eq 0 ]; then
      run_no_arg
    fi

    while [ $# -gt 0 ]; do
      case "$1" in
      install)
        run_install
        exit
        ;;
      up)
        run_up "$@"
        exit
        ;;
      down)
        run_down "$@"
        exit
        ;;
      validate)
        run_validate "$@"
        exit
        ;;
      verify)
        run_verify "$@"
        exit
        ;;
      *)
        run_args "$@"
        exit
        ;;
      esac
    done
  }

  check_os
  define_colors
  define_font_types

  run "$@"
}

main "$@"
