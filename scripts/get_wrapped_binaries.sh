#!/bin/bash
set -e
#!/bin/bash
set -e

download_and_move_binary() {
  local url=$1
  local binary=$2
  local target_dir=$3
  local target_name=$4
  local isTar=$5

  # check if tar
  if [ "$isTar" = true ]; then
    curl -LJ "$url" | tar xvz -C "$target_dir" "$binary"
    mv "$target_dir/$binary" "${target_dir}/$target_name"
  elif [ "$isTar" = false ]; then
    curl -LJ "$url" -o "$target_dir/$target_name"
  fi
}

download_and_move_binary "https://getbin.io/derailed/k9s?os=darwin&arch=amd64" "k9s" "src/KSail/assets/binaries" "k9s-darwin-amd64" true
download_and_move_binary "https://getbin.io/derailed/k9s?os=darwin&arch=arm64" "k9s" "src/KSail/assets/binaries" "k9s-darwin-arm64" true
download_and_move_binary "https://getbin.io/derailed/k9s?os=linux&arch=amd64" "usr/bin/k9s" "src/KSail/assets/binaries" "k9s-linux-amd64" true
download_and_move_binary "https://getbin.io/derailed/k9s?os=linux&arch=arm64" "usr/bin/k9s" "src/KSail/assets/binaries" "k9s-linux-arm64" true

download_and_move_binary "https://getbin.io/FiloSottile/age?os=darwin&arch=amd64" "age/age-keygen" "src/KSail/assets/binaries" "age-keygen-darwin-amd64" true
download_and_move_binary "https://getbin.io/FiloSottile/age?os=darwin&arch=arm64" "age/age-keygen" "src/KSail/assets/binaries" "age-keygen-darwin-arm64" true
download_and_move_binary "https://getbin.io/FiloSottile/age?os=linux&arch=amd64" "age/age-keygen" "src/KSail/assets/binaries" "age-keygen-linux-amd64" true
download_and_move_binary "https://getbin.io/FiloSottile/age?os=linux&arch=arm64" "age/age-keygen" "src/KSail/assets/binaries" "age-keygen-linux-arm64" true

download_and_move_binary "https://getbin.io/fluxcd/flux2?os=darwin&arch=amd64" "flux" "src/KSail/assets/binaries" "flux-darwin-amd64" true
download_and_move_binary "https://getbin.io/fluxcd/flux2?os=darwin&arch=arm64" "flux" "src/KSail/assets/binaries" "flux-darwin-arm64" true
download_and_move_binary "https://getbin.io/fluxcd/flux2?os=linux&arch=amd64" "flux" "src/KSail/assets/binaries" "flux-linux-amd64" true
download_and_move_binary "https://getbin.io/fluxcd/flux2?os=linux&arch=arm64" "flux" "src/KSail/assets/binaries" "flux-linux-arm64" true

download_and_move_binary "https://getbin.io/getsops/sops?os=darwin&arch=amd64" "sops" "src/KSail/assets/binaries" "sops-darwin-amd64" false
download_and_move_binary "https://getbin.io/getsops/sops?os=darwin&arch=arm64" "sops" "src/KSail/assets/binaries" "sops-darwin-arm64" false
download_and_move_binary "https://getbin.io/getsops/sops?os=linux&arch=amd64" "sops" "src/KSail/assets/binaries" "sops-linux-amd64" false
download_and_move_binary "https://getbin.io/getsops/sops?os=linux&arch=arm64" "sops" "src/KSail/assets/binaries" "sops-linux-arm64" false

download_and_move_binary "https://getbin.io/k3d-io/k3d?os=darwin&arch=amd64" "k3d" "src/KSail/assets/binaries" "k3d-darwin-amd64" false
download_and_move_binary "https://getbin.io/k3d-io/k3d?os=darwin&arch=arm64" "k3d" "src/KSail/assets/binaries" "k3d-darwin-arm64" false
download_and_move_binary "https://getbin.io/k3d-io/k3d?os=linux&arch=amd64" "k3d" "src/KSail/assets/binaries" "k3d-linux-amd64" false
download_and_move_binary "https://getbin.io/k3d-io/k3d?os=linux&arch=arm64" "k3d" "src/KSail/assets/binaries" "k3d-linux-arm64" false

download_and_move_binary "https://getbin.io/kubernetes-sigs/kind?os=darwin&arch=amd64" "kind" "src/KSail/assets/binaries" "kind-darwin-amd64" false
download_and_move_binary "https://getbin.io/kubernetes-sigs/kind?os=darwin&arch=arm64" "kind" "src/KSail/assets/binaries" "kind-darwin-arm64" false
download_and_move_binary "https://getbin.io/kubernetes-sigs/kind?os=linux&arch=amd64" "kind" "src/KSail/assets/binaries" "kind-linux-amd64" false
download_and_move_binary "https://getbin.io/kubernetes-sigs/kind?os=linux&arch=arm64" "kind" "src/KSail/assets/binaries" "kind-linux-arm64" false

curl -s "https://api.github.com/repos/kubernetes-sigs/kustomize/releases" | grep "browser_download.*darwin_amd64" | cut -d '"' -f 4 | sort -V | tail -n 1 | xargs curl -LJ | tar xvz -C src/KSail/assets/binaries kustomize
mv src/KSail/assets/binaries/kustomize src/KSail/assets/binaries/kustomize-darwin-amd64
curl -s "https://api.github.com/repos/kubernetes-sigs/kustomize/releases" | grep "browser_download.*darwin_arm64" | cut -d '"' -f 4 | sort -V | tail -n 1 | xargs curl -LJ | tar xvz -C src/KSail/assets/binaries kustomize
mv src/KSail/assets/binaries/kustomize src/KSail/assets/binaries/kustomize-darwin-arm64
curl -s "https://api.github.com/repos/kubernetes-sigs/kustomize/releases" | grep "browser_download.*linux_amd64" | cut -d '"' -f 4 | sort -V | tail -n 1 | xargs curl -LJ | tar xvz -C src/KSail/assets/binaries kustomize
mv src/KSail/assets/binaries/kustomize src/KSail/assets/binaries/kustomize-linux-amd64
curl -s "https://api.github.com/repos/kubernetes-sigs/kustomize/releases" | grep "browser_download.*linux_arm64" | cut -d '"' -f 4 | sort -V | tail -n 1 | xargs curl -LJ | tar xvz -C src/KSail/assets/binaries kustomize
mv src/KSail/assets/binaries/kustomize src/KSail/assets/binaries/kustomize-linux-arm64

download_and_move_binary "https://getbin.io/yannh/kubeconform?os=darwin&arch=amd64" "kubeconform" "src/KSail/assets/binaries" "kubeconform-darwin-amd64" true
download_and_move_binary "https://getbin.io/yannh/kubeconform?os=darwin&arch=arm64" "kubeconform" "src/KSail/assets/binaries" "kubeconform-darwin-arm64" true
download_and_move_binary "https://getbin.io/yannh/kubeconform?os=linux&arch=amd64" "kubeconform" "src/KSail/assets/binaries" "kubeconform-linux-amd64" true
download_and_move_binary "https://getbin.io/yannh/kubeconform?os=linux&arch=arm64" "kubeconform" "src/KSail/assets/binaries" "kubeconform-linux-arm64" true
