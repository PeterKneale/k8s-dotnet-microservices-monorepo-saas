#!/bin/bash -e

helm ls --short --namespace development | grep service | xargs -L1 helm delete --namespace development
helm ls --short --namespace development | grep frontend | xargs -L1 helm delete --namespace development
helm ls --short --namespace development | grep backend | xargs -L1 helm delete --namespace development