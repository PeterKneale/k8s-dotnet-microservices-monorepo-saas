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
[![backend_registration](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/backend_registration.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/backend_registration.yml)

[![backend_registration](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/backend_registration.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/backend_registration.yml)

[![frontend_admin](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_admin.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_admin.yml)

[![frontend_admin](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_admin.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_admin.yml)

[![frontend_marketing](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_marketing.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/
actions/workflows/frontend_marketing.yml)

[![frontend_registration](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_registration.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_registration.yml)

[![frontend_shopping](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_shopping.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/frontend_shopping.yml)

[![ingress_public](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/ingress_public.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/ingress_public.yml)

[![service_accounts](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_accounts.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/
workflows/service_accounts.yml)

[![service_accounts](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_accounts.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_accounts.yml)
[![service_catalog](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_catalog.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_catalog.yml)

[![service_catalog](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_catalog.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_catalog.yml)

[![service_search](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_search.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_search.yml)

[![service_stores](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_stores.yml/badge.svg)](https://github.com/PeterKneale/k8s-dotnet-microservices-monorepo-saas/actions/workflows/service_stores.yml)

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
docker-compose -f docker-compose-infra.yml up
```

- platform
```sh
docker-compose -f docker-compose.yml build
docker-compose -f docker-compose.yml -f docker-compose.override.yml up
docker-compose -f docker-compose.yml down
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

# Ingress
- [marketing.saas.io](http://marketing.saas.io)
- [registration.saas.io](http://registration.saas.io)
- [shopping.saas.io](http://shopping.saas.io)
- [management.saas.io](http://management.saas.io)
- [admin.saas.io](http://admin.saas.io)
- [example1.saas.io](http://example1.saas.io)
- [example2.saas.io](http://example2.saas.io)
- [example3.saas.io](http://example3.saas.io)
- [example1.io](http://example1.io)
- [example2.io](http://example2.io)
- [example3.io](http://example3.io)

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
  * `K8S_CLUSTER_NAME`
  * `REGISTRY_ENDPOINT` in the form `ghcr.io/peterkneale/k8s-dotnet-microservices-monorepo-saas`
  * `REGISTRY_HOST` in the form `ghcr.io`

# Release

## Setup

* Set environment variables
```sh
export GITHUB_USERNAME=peterkneale
export GITHUB_TOKEN=XXXXXXXXXXXXXX
export REGISTRY_HOST=ghcr.io
export REGISTRY_ENDPOINT=ghcr.io/peterkneale/k8s-dotnet-microservices-monorepo-saas
export TAG=latest
echo $GITHUB_TOKEN | docker login $REGISTRY_HOST -u $GITHUB_USERNAME --password-stdin
```
