# Using KSail in your CI pipeline

You need to download the KSail binary into your CI/CD environment, and then run the KSail commands as you would locally. For example, if you are using GitHub Actions, you can use the following workflow:

```yaml
name: KSail

on:
  pull_request:
    branches: [main]
  push:
    branches: [main]

concurrency:
  group: ${{ github.workflow }}-${{ github.ref }}
  cancel-in-progress: true

jobs:
  ksail:
    runs-on: ubuntu-latest
    steps:
      - name: 📑 Checkout
        uses: actions/checkout@v4
      - name: 🍺 Set up Homebrew
        uses: Homebrew/actions/setup-homebrew@master
      - name: 🛥️🐳 Install KSail
        run: brew install devantler/formulas/ksail
      - name: 🛥️🐳🚀 KSail Up
        run: |
          ksail sops <name-of-cluster> --import "${{ secrets.KSAIL_SOPS_AGE_KEY }}"
          ksail up <name-of-cluster>
```
