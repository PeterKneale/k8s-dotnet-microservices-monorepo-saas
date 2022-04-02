#!/bin/bash -e

# Remove old github credentials
kubectl delete secret github \
    --namespace saas-app-development \
    --ignore-not-found=true
kubectl delete secret github \
    --namespace saas-app-production \
    --ignore-not-found=true

# Install Github Pull Credentials (Development)
kubectl create secret docker-registry github \
    --namespace saas-app-development \
    --docker-server=$REGISTRY_HOST \
    --docker-username=$GITHUB_USERNAME \
    --docker-password=$GITHUB_TOKEN
    
# Install Github Pull Credentials (Production)
kubectl create secret docker-registry github \
    --namespace saas-app-production \
    --docker-server=$REGISTRY_HOST \
    --docker-username=$GITHUB_USERNAME \
    --docker-password=$GITHUB_TOKEN