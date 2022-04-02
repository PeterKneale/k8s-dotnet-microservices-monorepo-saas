#!/bin/bash -e

./k8s-install-infra.sh
./k8s-install-secret.sh
./k8s-install-app.sh
