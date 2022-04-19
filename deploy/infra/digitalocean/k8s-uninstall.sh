#!/bin/bash -e

echo "🔵 uninstalling..."

for env in development production
do    
    echo "✨ uninstalling $env"
    kubectl delete namespace $env --ignore-not-found=true
done

echo "✨ uninstalling config"
kubectl delete -f cert-manager-issuer.yaml