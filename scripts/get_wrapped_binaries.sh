#!/bin/bash
download_and_update() {
  repo=$1
  binary=$2
  is_tarball=$3
  unpackage=$4

  version_latest=$(curl -s https://api.github.com/repos/"$repo"/releases/latest | grep tag_name | cut -d '"' -f 4)
  if [ -z "$version_latest" ]; then
    echo "No version of $binary found, this is usually due to a rate limit on the GitHub API"
    return
  else
    # Check that version is semver vx.x.x or x.x.x
    if ! [[ "$version_latest" =~ ^v[0-9]+\.[0-9]+\.[0-9]+$ ]] && ! [[ "$version_latest" =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
      version_latest=$(echo "$version_latest" | cut -d '/' -f 2)
    fi
  fi
  if [ -f src/KSail/assets/binaries/requirements.txt ]; then
    version_current=$(grep "${binary}_version_" src/KSail/assets/binaries/requirements.txt | cut -d '_' -f 3)
  else
    version_current="0.0.0"
  fi
  if [ "$version_latest" != "$version_current" ]; then
    echo "New version of $binary found: $version_latest"
    echo "Current version of $binary: $version_current"
    echo "Downloading new version of $binary"
    for arch in darwin_amd64 darwin_arm64 darwin-amd64 darwin-arm64 linux_amd64 linux_arm64 linux-amd64 linux-arm64; do
      echo "Checking if $binary for $arch exists"
      local exists
      exists=$(curl -s https://api.github.com/repos/"$repo"/releases/latest | grep browser_download_url | grep $arch | cut -d '"' -f 4)
      if [ -z "$exists" ]; then
        echo "No $binary for $arch found"
        continue
      fi
      if [ "$is_tarball" = true ]; then
        curl -s https://api.github.com/repos/"$repo"/releases/latest | grep browser_download_url | grep $arch | cut -d '"' -f 4 | xargs curl -sL -o src/KSail/assets/binaries/"${binary}"_${arch}.tar.gz
        echo "Extracting new version of $binary"
        tar -xzf src/KSail/assets/binaries/"${binary}"_${arch}.tar.gz -C src/KSail/assets/binaries/
        # if not a folder, rename to binary_arch
        if [ ! -d src/KSail/assets/binaries/"${binary}" ]; then
          mv src/KSail/assets/binaries/"${binary}" src/KSail/assets/binaries/"${binary}"_${arch}
        fi
        echo "Removing tar.gz files"
        rm src/KSail/assets/binaries/"${binary}"_${arch}.tar.gz
      else
        curl -s https://api.github.com/repos/"$repo"/releases/latest | grep browser_download_url | grep $arch | cut -d '"' -f 4 | xargs curl -sL -o src/KSail/assets/binaries/"${binary}"_"${arch}"
      fi
      if [ "$unpackage" = true ]; then
        echo "Unpackaging new version of $binary"
        for file in src/KSail/assets/binaries/"${binary}"/*; do
          fileName=$(basename "$file")
          # if file is not LICENSE, rename to binary_arch
          if [ "$fileName" != "LICENSE" ]; then
            mv "$file" src/KSail/assets/binaries/"${fileName}"_"${arch}"
          fi
        done
        rm -rf src/KSail/assets/binaries/"${binary}"
      fi
      echo "Making new version of $binary executable"
      # Glob chmod +x ${binary}*
      find src/KSail/assets/binaries -name "${binary}*_${arch}" -type f -exec chmod +x {} \;
    done
    echo "Update version in requirements.txt"
    if [ ! -f src/KSail/assets/binaries/requirements.txt ]; then
      echo "${binary}_version_${version_latest}" >src/KSail/assets/binaries/requirements.txt
    elif grep -q "${binary}_version_${version_latest}" src/KSail/assets/binaries/requirements.txt; then
      sed -i '' "s/${binary}_version_*/${binary}_version_${version_latest}/g" src/KSail/assets/binaries/requirements.txt
    else
      echo "${binary}_version_${version_latest}" >>src/KSail/assets/binaries/requirements.txt
    fi
  else
    echo "No new version of $binary found"
  fi
}

set -e
# download_and_update "fluxcd/flux2" "flux" true false
# download_and_update "k3d-io/k3d" "k3d" false false
# download_and_update "yannh/kubeconform" "kubeconform" true false
# download_and_update "kubernetes-sigs/kustomize" "kustomize" true false
download_and_update "FiloSottile/age" "age" true true
find src/KSail/assets/binaries -name "LICENSE" -type f -delete
