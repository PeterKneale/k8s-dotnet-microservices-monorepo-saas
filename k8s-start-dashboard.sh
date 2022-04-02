#!/bin/bash -e

kubectl get secret $(kubectl get sa/admin-user -o jsonpath="{.secrets[0].name}") -o go-template="{{.data.token | base64decode}}"
echo
echo starting proxy
export POD_NAME=$(kubectl get pods -l "app.kubernetes.io/name=kubernetes-dashboard,app.kubernetes.io/instance=infra-dashboard" -o jsonpath="{.items[0].metadata.name}")
echo https://127.0.0.1:8443/
kubectl port-forward $POD_NAME 8443:8443