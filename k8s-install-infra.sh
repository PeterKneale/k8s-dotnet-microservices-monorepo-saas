#!/bin/bash -e

echo "🔵 installing..."

echo "✨ configuring helm repositories"

helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo add kubernetes-dashboard https://kubernetes.github.io/dashboard/
helm repo add kubernetes-ingress-nginx https://kubernetes.github.io/ingress-nginx/
helm repo add jetstack https://charts.jetstack.io
helm repo update

echo "✨ creating secrets"    
kubectl delete secret github --namespace $env --ignore-not-found=true

kubectl create secret docker-registry github \
    --namespace $env \
    --docker-server=$REGISTRY_HOST \
    --docker-username=$GITHUB_USERNAME \
    --docker-password=$GITHUB_TOKEN

echo "✨ creating pre-requisite config"

kubectl apply -f ./deploy/infra-dashboard-rbac.yaml
kubectl apply -f ./deploy/infra-dashboard-serviceaccount.yaml
kubectl apply -f ./deploy/infra-fluentd-config.yaml

echo "✨ creating infra"
helm install infra-dashboard kubernetes-dashboard/kubernetes-dashboard
helm install infra-ingress          kubernetes-ingress-nginx/ingress-nginx
helm install infra-cert-manager     jetstack/cert-manager --version v1.7.1 --set installCRDs=true
helm install infra-postgresql       bitnami/postgresql      -f ./deploy/infra-postgresql.yaml
helm install infra-rabbitmq         bitnami/rabbitmq        -f ./deploy/infra-rabbitmq.yaml
helm install infra-elasticsearch    bitnami/elasticsearch   -f ./deploy/infra-elasticsearch.yaml
helm install infra-kibana           bitnami/kibana          -f ./deploy/infra-kibana.yaml
helm install infra-minio            bitnami/minio           -f ./deploy/infra-minio.yaml
helm install infra-fluentd          bitnami/fluentd         -f ./deploy/infra-fluentd.yaml

echo "✨ installing post-requisite config"
kubectl apply -f ./deploy/infra-cert-manager-cert.yaml
kubectl apply -f ./deploy/infra-cert-manager-issuer.yaml

echo "✔ installed $env"