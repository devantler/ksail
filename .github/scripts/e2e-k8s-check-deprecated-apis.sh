release=$(curl -sL "https://api.github.com/repos/doitintl/kube-no-trouble/releases/latest" |
  grep -oE '"tag_name": "[^"]*' |
  grep -oE '[^"]*$')
os=$(uname -s | tr '[:upper:]' '[:lower:]')
arch=$(uname -m | sed 's/x86_64/amd64/;s/aarch64/arm64/')
curl -L -o- "https://github.com/doitintl/kube-no-trouble/releases/download/${release}/kubent-${release}-${os}-${arch}.tar.gz" | tar xvzf -
./kubent -e
