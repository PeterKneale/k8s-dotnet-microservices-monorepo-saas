#!/bin/bash -e

# Create namespaces for frontends, backends and services
kubectl create namespace saas-app-development --dry-run=client -o yaml | kubectl apply -f -
kubectl create namespace saas-app-production --dry-run=client -o yaml | kubectl apply -f -

# Create namespaces for infrastructure like postgres, rabbitmq and minio
kubectl create namespace saas-infra-development --dry-run=client -o yaml | kubectl apply -f -
kubectl create namespace saas-infra-production --dry-run=client -o yaml | kubectl apply -f -