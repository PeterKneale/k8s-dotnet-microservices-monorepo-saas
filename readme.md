# k8s-dotnet-microservices-monorepo-saas

A work in progress demonstrating
- k8s deployments to digital ocean using helm charts
- microservices using dotnet
- CI-CD using github actions
- monorepo
- to build a demo software as a service application

<!-- vscode-markdown-toc -->

*
    1. [Sites](#Sites)

    * 1.1. [Marketing Website Overview](#MarketingWebsiteOverview)
    * 1.2. [Registration Website Overview](#RegistrationWebsiteOverview)
    * 1.3. [Management Website Overview](#ManagementWebsiteOverview)
    * 1.4. [Sales Website Overview](#SalesWebsiteOverview)
    * 1.5. [Admin Website Overview](#AdminWebsiteOverview)
*
    2. [Gateways](#Gateways)

    * 2.1. [Sales Gateway](#SalesGateway)
    * 2.2. [Management Gateway](#ManagementGateway)
*
    3. [Microservices](#Microservices)

    * 3.1. [Users Microservice](#UsersMicroservice)
    * 3.2. [Stores Microservice](#StoresMicroservice)
    * 3.3. [Catalog Microservice](#CatalogMicroservice)
    * 3.4. [Carts Microservice](#CartsMicroservice)
    * 3.5. [Search Microservice](#SearchMicroservice)
    * 3.6. [Orders Microservice](#OrdersMicroservice)

<!-- vscode-markdown-toc-config
	numbering=true
	autoSave=true
	/vscode-markdown-toc-config -->
<!-- /vscode-markdown-toc -->

## Builds

[![ingress_public](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/ingress_public.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/ingress_public.yaml)

[![frontend_admin](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_admin.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_admin.yaml)

[![frontend_marketing](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_marketing.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_marketing.yaml)

[![frontend_management](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_management.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_marketing.yaml)

[![frontend_registration](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_registration.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_registration.yaml)

[![frontend_shopping](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_shopping.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_shopping.yaml)

[![backend_registration](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/backend_registration.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/backend_registration.yaml)

[![backend_shopping](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/backend_shopping.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/backend_registration.yaml)

[![service_accounts](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_accounts.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_accounts.yaml)

[![service_accounts](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_accounts.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_accounts.yaml)

[![service_catalog](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_catalog.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_catalog.yaml)

[![service_catalog](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_catalog.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_catalog.yaml)

[![service_search](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_search.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_search.yaml)

[![service_stores](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_stores.yaml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_stores.yaml)

# Environments

## Development

The top level domain is `ecommerce-store-builder.dev`
- [marketing](http://marketing.ecommerce-store-builder.dev)
- [registration](http://registration.ecommerce-store-builder.dev)
- [shopping](http://shopping.ecommerce-store-builder.dev)
- [management](http://management.ecommerce-store-builder.dev)
- [admin](http://admin.ecommerce-store-builder.dev)

Example stores
- [example1](http://example1.ecommerce-store-builder.dev)
- [example2](http://example2.ecommerce-store-builder.dev)
- [example3](http://example3.ecommerce-store-builder.dev)

Custom domain names for example stores
- [example-store-1.xyz](http://example-store-1.xyz)
- [example-store-2.xyz](http://example-store-2.xyz)
- [example-store-3.xyz](http://example-store-3.xyz)

## Production  

The top level domain is `ecommerce-store-builder.com`
- [marketing](http://marketing.ecommerce-store-builder.com)
- [registration](http://registration.ecommerce-store-builder.com)
- [shopping](http://shopping.ecommerce-store-builder.com)
- [management](http://management.ecommerce-store-builder.com)
- [admin](http://admin.ecommerce-store-builder.com)

Example stores
- [example4](http://example4.ecommerce-store-builder.com)
- [example5](http://example5.ecommerce-store-builder.com)
- [example6](http://example6.ecommerce-store-builder.com)

Custom domain names for example stores
- [example-store-4.xyz](http://example-store-4.xyz)
- [example-store-5.xyz](http://example-store-5.xyz)
- [example-store-6.xyz](http://example-store-6.xyz)

[![DigitalOcean Referral Badge](https://web-platforms.sfo2.digitaloceanspaces.com/WWW/Badge%202.svg)](https://www.digitalocean.com/?refcode=86a9c64ead8b&utm_campaign=Referral_Invite&utm_medium=Referral_Program&utm_source=badge)

# Solution Diagrams

# Context

![Docs](/docs/images/c4-context.png)

![Docs](/docs/images/c4-context-detailed.png)

client -> ingress -> frontend -> backend -> service

## 1. <a name='Sites'></a>Sites

### 1.1. <a name='MarketingWebsiteOverview'></a>Marketing Website Overview

![Docs](/docs/images/c4-container-marketing.png)

### 1.2. <a name='RegistrationWebsiteOverview'></a>Registration Website Overview

![Docs](/docs/images/c4-container-registration.png)

### 1.3. <a name='ManagementWebsiteOverview'></a>Management Website Overview

![Docs](/docs/images/c4-container-management.png)

### 1.4. <a name='SalesWebsiteOverview'></a>Sales Website Overview

![Docs](/docs/images/c4-container-sales.png)

### 1.5. <a name='AdminWebsiteOverview'></a>Admin Website Overview

![Docs](/docs/images/c4-container-admin.png)

## 2. <a name='Gateways'></a>Gateways

### 2.1. <a name='SalesGateway'></a>Sales Gateway

### 2.2. <a name='ManagementGateway'></a>Management Gateway

## 3. <a name='Microservices'></a>Microservices
###  3.1. <a name='AccountsMicroservice'></a>Accounts Microservice
Use Cases
- Create account
- Add user to account
- Auth user

Publishes
- Account Created
- User Created

Subscribes
- None

###  3.2. <a name='StoresMicroservice'></a>Stores Microservice

Use Cases
- Create store
- Get store
- Get store by domain
- Set store theme
- Set store custom domain



### 3.1. <a name='UsersMicroservice'></a>Users Microservice

```
API
- Create                (userId, email, password)  [Anonymous]
- Auth                  (email, password)          [Anonymous]

Application

Domain
- User

Infrastructure
- CurrentSiteContext
- CurrentUserContext

Publishes
- UserCreated

Subscribes
```

### 3.2. <a name='StoresMicroservice'></a>Stores Microservice

```
API
- Create                (storeId, name)             [Anonymous] 
- GetById               (storeId)                   [Anonymous]
- GetByDomain           (domain)                    [Anonymous]
- SetTheme              (storeId, theme)            [Owner]
- SetCustomDomain       (storeId, domain)           [Owner]
- List                  ()                          [Admin]

Application

Domain
- Cart
- CartItem

Infrastructure

Publishes
- StoreCreated
- StoreUpdated

Subscribes
```

### 3.3. <a name='CatalogMicroservice'></a>Products Microservice

### 3.4. <a name='CartsMicroservice'></a>Carts Microservice

### 3.5. <a name='SearchMicroservice'></a>Search Microservice

### 3.6. <a name='MediaMicroservice'></a>Media Microservice

# Running in docker

- infra
```sh
docker-compose -f docker-compose-infra.yaml up
```

- platform
```sh
docker-compose -f docker-compose.yaml build
docker-compose -f docker-compose.yaml -f docker-compose.override.yaml up
docker-compose -f docker-compose.yaml down
```
# Development

## Host file setup

```txt
127.0.0.1 marketing.saas.io
127.0.0.1 registration.saas.io
127.0.0.1 shopping.saas.io
127.0.0.1 management.saas.io
127.0.0.1 admin.saas.io
127.0.0.1 example1.saas.io
127.0.0.1 example2.saas.io
127.0.0.1 example3.saas.io
127.0.0.1 example1.io
127.0.0.1 example2.io
127.0.0.1 example3.io
```

## Port registry

FrontEnds
- marketing		8010
- registration	8020
- shopping		8030
- management	8040
- admin			8050

Gateways
- marketing		7010
- registration	7020
- shopping		7030
- management	7040
- admin			7050

Backends
- registration	6020
- shopping		6030

Services
- catalog 	5010
- carts		5020
- media	 	5030
- search 	5040
- stores 	5050
- accounts 	5060

# Build

## Github Actions
 
* Setup Secrets
  * `DIGITALOCEAN_ACCESS_TOKEN`
  * `REGISTRY_ENDPOINT` in the form `ghcr.io/peterkneale/k8s-dotnet-microservices-monorepo-saas`
  * `REGISTRY_HOST` in the form `ghcr.io`

# Release

## Setup

* Set environment variables
```sh
export GITHUB_USERNAME=peterkneale
export GITHUB_TOKEN=XXXXXXXXXXXXXX
export DIGITALOCEAN_TOKEN=XXXXXXXXXXXXXX
export REGISTRY_HOST=ghcr.io
export REGISTRY_ENDPOINT=ghcr.io/peterkneale/k8s-dotnet-microservices-monorepo-saas
export TAG=latest
echo $GITHUB_TOKEN | docker login $REGISTRY_HOST -u $GITHUB_USERNAME --password-stdin
echo $DIGITALOCEAN_TOKEN | doctl auth init --context saas

```

# K8s
## Accessing cluster resources

Accessing a resource in the k8s cluster can be performed by using kubectl's port forwarding feature
Forwarding the local port 8080 to the below services will make them available on [http://localhost:8080](http://localhost:8080)

- Kibana
    ```shell
    kubectl port-forward svc/infra-kibana 8080:5601
    ```

- Prometheus Web Pane
    ```shell
    kubectl port-forward svc/kube-prometheus-stack-prometheus 8080:9090 -n kube-prometheus-stack
    ```

- Grafana Web Panel
    ```shell
    kubectl port-forward svc/kube-prometheus-stack-grafana 8080:80 -n kube-prometheus-stack
    ```

- A service
    ```shell
    kubectl port-forward svc/service-accounts-saas 8080:80
    ```
    ```shell
    $ curl localhost:8080/health/alive
    Healthy
    ```
