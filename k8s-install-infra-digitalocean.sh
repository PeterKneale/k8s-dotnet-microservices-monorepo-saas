#!/bin/bash -e

echo "🔵 installing..."

echo "✨ configuring helm repositories"
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update

for env in saas-infra-development saas-infra-production
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
    kubectl apply --namespace $env -f ./deploy/infra-fluentd-config.yaml
        
    echo "✨ creating infra"
    helm upgrade --install --namespace $env infra-postgresql       bitnami/postgresql      -f ./deploy/infra-postgresql.yaml
    helm upgrade --install --namespace $env infra-rabbitmq         bitnami/rabbitmq        -f ./deploy/infra-rabbitmq.yaml
    helm upgrade --install --namespace $env infra-elasticsearch    bitnami/elasticsearch   -f ./deploy/infra-elasticsearch.yaml
    helm upgrade --install --namespace $env infra-kibana           bitnami/kibana          -f ./deploy/infra-kibana.yaml
    helm upgrade --install --namespace $env infra-minio            bitnami/minio           -f ./deploy/infra-minio.yaml
    helm upgrade --install --namespace $env infra-fluentd          bitnami/fluentd         -f ./deploy/infra-fluentd.yaml
    
    echo "✔ installed $env"
done
