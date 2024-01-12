echo "-------- 📝 LOGS 📝 --------"
echo "📝 Flux logs"
flux logs --all-namespaces

echo -e "\n"
echo "📝 kubectl logs - source-controller"
kubectl -n flux-system logs deploy/source-controller

echo -e "\n"
echo "📝 kubectl logs - kustomize-controller"
kubectl -n flux-system logs deploy/kustomize-controller

echo -e "\n"
echo "📝 kubectl logs - helm-controller"
kubectl -n flux-system logs deploy/helm-controller

echo -e "\n"
echo "📝 kubectl logs - notification-controller"
kubectl -n flux-system logs deploy/notification-controller
