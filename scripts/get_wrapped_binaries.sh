#!/bin/bash
download_and_update() {
  repo=$1
  binary=$2
  is_tarball=$3
  subfolder=$4
  architectures=("darwin.amd64" "darwin.arm64" "linux.amd64" "linux.arm64")

  echo "Fetching latest release information for $repo"
  latest_release=$(curl -s https://api.github.com/repos/"$repo"/releases/latest)
  version_latest=$(echo "$latest_release" | grep tag_name | cut -d '"' -f 4 | cut -d '/' -f 2)
  version_current=$(grep -s "${binary}_version_" src/KSail/assets/binaries/requirements.txt | cut -d '_' -f 3)
  if [ -z "$version_current" ]; then
    version_current="v0.0.0"
  fi
  echo "Latest version: $version_latest"
  echo "Current version: $version_current"

  if [ "$version_latest" != "$version_current" ]; then
    echo "Updating $binary to version $version_latest"
    for arch in "${architectures[@]}"; do
      arch=${arch//./-}
      arch_underscore=${arch//-/_}
      exists=$(echo "$latest_release" | grep browser_download_url | grep -E "(${arch}|${arch_underscore})\"" | cut -d '"' -f 4)
      if [ -n "$exists" ]; then
        echo "Downloading $binary for architecture $arch"
        curl -sL -o src/KSail/assets/binaries/"${binary}"_"${arch}""$([ "$is_tarball" = true ] && echo ".tar.gz")" "$exists"
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
    echo "Updating version in requirements.txt"
    if [ "$version_current" = "v0.0.0" ]; then
      echo "${binary}_version_${version_latest}" >>src/KSail/assets/binaries/requirements.txt
    else
      sed -i'' -e "s/^${binary}_version_.*/${binary}_version_${version_latest}/" src/KSail/assets/binaries/requirements.txt
    fi
  else
    echo "$binary is already up to date"
    echo ""
  fi
}

set -e
download_and_update "fluxcd/flux2" "flux" true
download_and_update "k3d-io/k3d" "k3d" false
download_and_update "kubernetes-sigs/kind" "kind" false
download_and_update "yannh/kubeconform" "kubeconform" true
download_and_update "kubernetes-sigs/kustomize" "kustomize" true
download_and_update "FiloSottile/age" "age-keygen" true "age"
download_and_update "getsops/sops" "sops" false
echo "Removing LICENSE files"
find src/KSail/assets/binaries -name "LICENSE" -type f -delete
