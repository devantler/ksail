FROM ubuntu:24.04

ARG ARCH=arm64
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

RUN apt-get update && \
  apt-get install -y curl jq sudo && \
  curl -fsSL https://get.docker.com -o get-docker.sh && \
  sudo sh get-docker.sh && \
  LATEST_RELEASE=$(curl --silent "https://api.github.com/repos/devantler/ksail/releases/latest" | jq -r .tag_name) && \
  curl -L -o ksail "https://github.com/devantler/ksail/releases/download/${LATEST_RELEASE}/ksail-linux-${ARCH}" && \
  chmod +x ksail && \
  mv ksail /usr/local/bin/

WORKDIR /app
ENTRYPOINT [ "ksail" ]
