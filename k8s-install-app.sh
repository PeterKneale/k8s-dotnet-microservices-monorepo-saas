#!/bin/bash -e

helm install service-accounts   ./deploy/chart -f ./deploy/service.yaml  -f ./deploy/service-accounts.yaml  
helm install service-carts      ./deploy/chart -f ./deploy/service.yaml  -f ./deploy/service-carts.yaml  
helm install service-catalog    ./deploy/chart -f ./deploy/service.yaml  -f ./deploy/service-catalog.yaml  
helm install service-media      ./deploy/chart -f ./deploy/service.yaml  -f ./deploy/service-media.yaml  
helm install service-search     ./deploy/chart -f ./deploy/service.yaml  -f ./deploy/service-search.yaml  
helm install service-stores     ./deploy/chart -f ./deploy/service.yaml  -f ./deploy/service-stores.yaml  

helm install backend-shopping       ./deploy/chart -f ./deploy/backend.yaml  -f ./deploy/backend-shopping.yaml  
helm install backend-registration   ./deploy/chart -f ./deploy/backend.yaml  -f ./deploy/backend-registration.yaml

helm install frontend-admin         ./deploy/chart -f ./deploy/frontend.yaml -f ./deploy/frontend-admin.yaml  
helm install frontend-management    ./deploy/chart -f ./deploy/frontend.yaml -f ./deploy/frontend-management.yaml  
helm install frontend-marketing     ./deploy/chart -f ./deploy/frontend.yaml -f ./deploy/frontend-marketing.yaml  
helm install frontend-registration  ./deploy/chart -f ./deploy/frontend.yaml -f ./deploy/frontend-registration.yaml  
helm install frontend-shopping      ./deploy/chart -f ./deploy/frontend.yaml -f ./deploy/frontend-shopping.yaml  
