echo "-------- 📝 ERRORS 📝 --------"
if flux logs --all-namespaces --level error | grep -i 'error'; then
  echo "📝 Flux logs - errors"
  flux logs --all-namespaces --level error | grep -i 'error'
fi

if kubectl -n flux-system logs deploy/source-controller | grep -i 'error'; then
  echo -e "\n"
  echo "📝 kubectl logs - source-controller errors"
  kubectl -n flux-system logs deploy/source-controller | grep -i 'error'
fi

if kubectl -n flux-system logs deploy/kustomize-controller | grep -i 'error'; then
  echo -e "\n"
  echo "📝 Flux logs - kustomize-controller errors"
  kubectl -n flux-system logs deploy/kustomize-controller | grep -i 'error'
fi

if kubectl -n flux-system logs deploy/helm-controller | grep -i 'error'; then
  echo -e "\n"
  echo "📝 Flux logs - helm-controller errors"
  kubectl -n flux-system logs deploy/helm-controller | grep -i 'error'
fi

if kubectl -n flux-system logs deploy/notification-controller | grep -i 'error'; then
  echo -e "\n"
  echo "📝 Flux logs - notification-controller errors"
  kubectl -n flux-system logs deploy/notification-controller | grep -i 'error'
fi
