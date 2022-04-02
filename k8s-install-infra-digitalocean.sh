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
    kubectl apply --namespace $env -f ./deploy/infra/infra-fluentd-config.yaml
        
    echo "✨ creating infra"
    helm upgrade --install --namespace $env infra-postgresql       bitnami/postgresql      -f ./deploy/infra/infra-postgresql.yaml
    helm upgrade --install --namespace $env infra-rabbitmq         bitnami/rabbitmq        -f ./deploy/infra/infra-rabbitmq.yaml
    helm upgrade --install --namespace $env infra-elasticsearch    bitnami/elasticsearch   -f ./deploy/infra/infra-elasticsearch.yaml
    helm upgrade --install --namespace $env infra-kibana           bitnami/kibana          -f ./deploy/infra/infra-kibana.yaml
    helm upgrade --install --namespace $env infra-minio            bitnami/minio           -f ./deploy/infra/infra-minio.yaml
    helm upgrade --install --namespace $env infra-fluentd          bitnami/fluentd         -f ./deploy/infra/infra-fluentd.yaml
    
    echo "✔ installed $env"
done
