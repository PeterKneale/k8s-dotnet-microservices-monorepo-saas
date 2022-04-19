#!/bin/bash -e

echo "🔵 installing..."

echo "✨ configuring helm repositories"
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update

for env in development production
do
    echo "🔵 installing $env"

    echo "✨ creating namespaces"
    kubectl create namespace $env --dry-run=client -o yaml | kubectl apply -f -

    echo "✨ creating secrets"    
    kubectl delete secret github --namespace $env --ignore-not-found=true

    kubectl create secret docker-registry github \
        --namespace $env \
        --docker-server=$REGISTRY_HOST \
        --docker-username=$GITHUB_USERNAME \
        --docker-password=$GITHUB_TOKEN
    
    echo "✨ creating configs"
    kubectl apply --namespace $env -f fluentd-config.yaml

    echo "✨ installing certificate issuer"
    kubectl apply -f cert-manager-issuer.yaml --namespace $env

    echo "✨ creating infra"
    helm upgrade --install --namespace $env infra-elasticsearch    bitnami/elasticsearch   -f elasticsearch.yaml
    helm upgrade --install --namespace $env infra-postgresql       bitnami/postgresql      -f postgresql.yaml
    helm upgrade --install --namespace $env infra-rabbitmq         bitnami/rabbitmq        -f rabbitmq.yaml
    helm upgrade --install --namespace $env infra-minio            bitnami/minio           -f minio.yaml
    
    echo "✔ installed $env"
done