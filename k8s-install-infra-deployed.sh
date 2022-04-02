#!/bin/bash -e

# add repos
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update

# Pre-requisites
kubectl apply -f ./deploy/infra-fluentd-config.yaml

# Infrastructure to install
helm install infra-postgresql       bitnami/postgresql      -f ./deploy/infra-postgresql.yaml
helm install infra-rabbitmq         bitnami/rabbitmq        -f ./deploy/infra-rabbitmq.yaml
helm install infra-elasticsearch    bitnami/elasticsearch   -f ./deploy/infra-elasticsearch.yaml
helm install infra-kibana           bitnami/kibana          -f ./deploy/infra-kibana.yaml
helm install infra-minio            bitnami/minio           -f ./deploy/infra-minio.yaml
helm install infra-fluentd          bitnami/fluentd         -f ./deploy/infra-fluentd.yaml