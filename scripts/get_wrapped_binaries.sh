#!/bin/bash
download_and_update() {
  repo=$1
  binary=$2
  is_tarball=$3
  subfolder=$4
  architectures=("darwin-amd64" "darwin_amd64" "darwin-arm64" "darwin_arm64" "linux-amd64" "linux_amd64" "linux-arm64" "linux_arm64")
  latest_release=$(curl -s https://api.github.com/repos/"$repo"/releases/latest)
  version_latest=$(echo "$latest_release" | grep tag_name | cut -d '"' -f 4 | cut -d '/' -f 2)

  for arch in "${architectures[@]}"; do
    exists=$(echo "$latest_release" | grep browser_download_url | grep "$arch" | cut -d '"' -f 4)
    if [ -n "$exists" ]; then
      echo "Downloading $binary $version_latest for architecture $arch"
      curl -s -L "$exists" -o src/KSail/assets/binaries/"${binary}"_"${arch}""$([ "$is_tarball" = true ] && echo ".tar.gz")"
      if [ "$is_tarball" = true ]; then
        echo "Extracting tarball"
        tar -xzf src/KSail/assets/binaries/"${binary}"_"${arch}".tar.gz -C src/KSail/assets/binaries/
        rm src/KSail/assets/binaries/"${binary}"_"${arch}".tar.gz
      fi
      if [ -n "$subfolder" ]; then
        echo "Moving binary from subfolder $subfolder"
        mv -f src/KSail/assets/binaries/"${subfolder}"/"$binary" src/KSail/assets/binaries/"${binary}"_"${arch}"
        rm -rf src/KSail/assets/binaries/"${subfolder}"
      elif [ -e src/KSail/assets/binaries/"$binary" ]; then
        mv -f src/KSail/assets/binaries/"$binary" src/KSail/assets/binaries/"${binary}"_"${arch}"
      fi
      chmod +x src/KSail/assets/binaries/"${binary}"_"${arch}"
    fi
  done
  find src/KSail/assets/binaries -name "LICENSE" -type f -delete
}

set -e
download_and_update "fluxcd/flux2" "flux" true
download_and_update "k3d-io/k3d" "k3d" false
download_and_update "kubernetes-sigs/kind" "kind" false
download_and_update "yannh/kubeconform" "kubeconform" true
download_and_update "kubernetes-sigs/kustomize" "kustomize" true
download_and_update "FiloSottile/age" "age-keygen" true "age"
download_and_update "getsops/sops" "sops" false
