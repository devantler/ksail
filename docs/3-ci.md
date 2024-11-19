# Using KSail in your CI pipeline

> [!NOTE]
> I plan to create a GitHub Action for KSail, so it becomes even easier to use KSail in your CI/CD pipeline.

KSail is built to be used in your CI/CD pipeline, so you can test and validate your Kubernetes manifests before deploying them to your other environments. To run KSail in your CI/CD pipeline you need to download the KSail binary into your CI/CD environment, and then run the KSail commands as you would locally. For example, if you are using GitHub Actions, you can use the following workflow:

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

As your cluster grows in size you might outgrow the resources available in your CI/CD runner. In that case you can consider self-hosting CI/CD runners with more resources to meet the vertical scaling requirements of your KSail clusters.

> [!NOTE]
> In the future I plan to add support for the VCluster technology, so you also have the option to scale your KSail clusters horizontally given that you have a Kubernetes cluster available to run them on.
