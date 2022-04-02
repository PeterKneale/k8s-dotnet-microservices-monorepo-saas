#!/bin/bash -e

# Install Github Pull Credentials
kubectl delete secret github --ignore-not-found=true
kubectl create secret docker-registry github --docker-server=$REGISTRY_HOST --docker-username=$GITHUB_USERNAME --docker-password=$GITHUB_TOKEN