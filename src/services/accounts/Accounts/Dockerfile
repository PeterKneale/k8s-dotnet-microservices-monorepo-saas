FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 81

# add globalization support
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# add diagnostic tools
RUN apk add --no-cache curl

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /work

# Start build cache
COPY "saas.sln" "saas.sln"

# frontends
COPY "src/frontends/admin/Admin/Admin.csproj" "src/frontends/admin/Admin/Admin.csproj"
COPY "src/frontends/admin/Admin.UnitTests/Admin.UnitTests.csproj" "src/frontends/admin/Admin.UnitTests/Admin.UnitTests.csproj"
COPY "src/frontends/admin/Admin.FunctionalTests/Admin.FunctionalTests.csproj" "src/frontends/admin/Admin.FunctionalTests/Admin.FunctionalTests.csproj"

COPY "src/frontends/management/Management/Management.csproj" "src/frontends/management/Management/Management.csproj"
COPY "src/frontends/management/Management.UnitTests/Management.UnitTests.csproj" "src/frontends/management/Management.UnitTests/Management.UnitTests.csproj"
COPY "src/frontends/management/Management.FunctionalTests/Management.FunctionalTests.csproj" "src/frontends/management/Management.FunctionalTests/Management.FunctionalTests.csproj"

COPY "src/frontends/registration/Registration/Registration.csproj" "src/frontends/registration/Registration/Registration.csproj"
COPY "src/frontends/registration/Registration.UnitTests/Registration.UnitTests.csproj" "src/frontends/registration/Registration.UnitTests/Registration.UnitTests.csproj"
COPY "src/frontends/registration/Registration.FunctionalTests/Registration.FunctionalTests.csproj" "src/frontends/registration/Registration.FunctionalTests/Registration.FunctionalTests.csproj"

COPY "src/frontends/shopping/Shopping/Shopping.csproj" "src/frontends/shopping/Shopping/Shopping.csproj"
COPY "src/frontends/shopping/Shopping.UnitTests/Shopping.UnitTests.csproj" "src/frontends/shopping/Shopping.UnitTests/Shopping.UnitTests.csproj"
COPY "src/frontends/shopping/Shopping.FunctionalTests/Shopping.FunctionalTests.csproj" "src/frontends/shopping/Shopping.FunctionalTests/Shopping.FunctionalTests.csproj"

# backends
COPY "src/backends/registration/Registration/Registration.csproj" "src/backends/registration/Registration/Registration.csproj"
COPY "src/backends/registration/Registration.UnitTests/Registration.UnitTests.csproj" "src/backends/registration/Registration.UnitTests/Registration.UnitTests.csproj"
COPY "src/backends/registration/Registration.FunctionalTests/Registration.FunctionalTests.csproj" "src/backends/registration/Registration.FunctionalTests/Registration.FunctionalTests.csproj"

COPY "src/backends/shopping/Shopping/Shopping.csproj" "src/backends/shopping/Shopping/Shopping.csproj"
COPY "src/backends/shopping/Shopping.UnitTests/Shopping.UnitTests.csproj" "src/backends/shopping/Shopping.UnitTests/Shopping.UnitTests.csproj"
COPY "src/backends/shopping/Shopping.FunctionalTests/Shopping.FunctionalTests.csproj" "src/backends/shopping/Shopping.FunctionalTests/Shopping.FunctionalTests.csproj"

# services
COPY "src/services/carts/Carts/Carts.csproj" "src/services/carts/Carts/Carts.csproj"
COPY "src/services/carts/Carts.UnitTests/Carts.UnitTests.csproj" "src/services/carts/Carts.UnitTests/Carts.UnitTests.csproj"
COPY "src/services/carts/Carts.FunctionalTests/Carts.FunctionalTests.csproj" "src/services/carts/Carts.FunctionalTests/Carts.FunctionalTests.csproj"

COPY "src/services/catalog/Catalog/Catalog.csproj" "src/services/catalog/Catalog/Catalog.csproj"
COPY "src/services/catalog/Catalog.UnitTests/Catalog.UnitTests.csproj" "src/services/catalog/Catalog.UnitTests/Catalog.UnitTests.csproj"
COPY "src/services/catalog/Catalog.FunctionalTests/Catalog.FunctionalTests.csproj" "src/services/catalog/Catalog.FunctionalTests/Catalog.FunctionalTests.csproj"

COPY "src/services/media/Media/Media.csproj" "src/services/media/Media/Media.csproj"
COPY "src/services/media/Media.UnitTests/Media.UnitTests.csproj" "src/services/media/Media.UnitTests/Media.UnitTests.csproj"
COPY "src/services/media/Media.FunctionalTests/Media.FunctionalTests.csproj" "src/services/media/Media.FunctionalTests/Media.FunctionalTests.csproj"

COPY "src/services/search/Search/Search.csproj" "src/services/search/Search/Search.csproj"
COPY "src/services/search/Search.UnitTests/Search.UnitTests.csproj" "src/services/search/Search.UnitTests/Search.UnitTests.csproj"
COPY "src/services/search/Search.FunctionalTests/Search.FunctionalTests.csproj" "src/services/search/Search.FunctionalTests/Search.FunctionalTests.csproj"

COPY "src/services/stores/Stores/Stores.csproj" "src/services/stores/Stores/Stores.csproj"
COPY "src/services/stores/Stores.UnitTests/Stores.UnitTests.csproj" "src/services/stores/Stores.UnitTests/Stores.UnitTests.csproj"
COPY "src/services/stores/Stores.FunctionalTests/Stores.FunctionalTests.csproj" "src/services/stores/Stores.FunctionalTests/Stores.FunctionalTests.csproj"

COPY "src/services/accounts/Accounts/Accounts.csproj" "src/services/accounts/Accounts/Accounts.csproj"
COPY "src/services/accounts/Accounts.UnitTests/Accounts.UnitTests.csproj" "src/services/accounts/Accounts.UnitTests/Accounts.UnitTests.csproj"
COPY "src/services/accounts/Accounts.FunctionalTests/Accounts.FunctionalTests.csproj" "src/services/accounts/Accounts.FunctionalTests/Accounts.FunctionalTests.csproj"

# Building blocks
COPY "src/building_blocks/domain/BuildingBlocks.Domain.DDD/BuildingBlocks.Domain.DDD.csproj" "src/building_blocks/domain/BuildingBlocks.Domain.DDD/BuildingBlocks.Domain.DDD.csproj"
COPY "src/building_blocks/application/BuildingBlocks.Application/BuildingBlocks.Application.csproj" "src/building_blocks/application/BuildingBlocks.Application/BuildingBlocks.Application.csproj"
COPY "src/building_blocks/infrastructure/BuildingBlocks.Infrastructure/BuildingBlocks.Infrastructure.csproj" "src/building_blocks/infrastructure/BuildingBlocks.Infrastructure/BuildingBlocks.Infrastructure.csproj"
COPY "src/building_blocks/infrastructure/BuildingBlocks.Infrastructure.UnitTests/BuildingBlocks.Infrastructure.UnitTests.csproj" "src/building_blocks/infrastructure/BuildingBlocks.Infrastructure.UnitTests/BuildingBlocks.Infrastructure.UnitTests.csproj"
# Messages
COPY "src/services/carts/Carts.Messages/Carts.Messages.csproj" "src/services/carts/Carts.Messages/Carts.Messages.csproj"
COPY "src/services/catalog/Catalog.Messages/Catalog.Messages.csproj" "src/services/catalog/Catalog.Messages/Catalog.Messages.csproj"
COPY "src/services/media/Media.Messages/Media.Messages.csproj" "src/services/media/Media.Messages/Media.Messages.csproj"
COPY "src/services/search/Search.Messages/Search.Messages.csproj" "src/services/search/Search.Messages/Search.Messages.csproj"
COPY "src/services/stores/Stores.Messages/Stores.Messages.csproj" "src/services/stores/Stores.Messages/Stores.Messages.csproj"
COPY "src/services/accounts/Accounts.Messages/Accounts.Messages.csproj" "src/services/accounts/Accounts.Messages/Accounts.Messages.csproj"
# others
COPY "tests/SystemTests/SystemTests.csproj" "tests/SystemTests/SystemTests.csproj"
COPY "tools/GenerateDockerFiles/GenerateDockerFiles.csproj" "tools/GenerateDockerFiles/GenerateDockerFiles.csproj"
# End build cache

RUN dotnet restore "saas.sln"
COPY . .
WORKDIR /work/src/services/accounts/Accounts
RUN dotnet publish --no-restore -c Release -o /app

FROM build as unit_tests
WORKDIR /work/src/services/accounts/Accounts.UnitTests

FROM build as functional_tests
WORKDIR /work/src/services/accounts/Accounts.FunctionalTests

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Accounts.dll"]
