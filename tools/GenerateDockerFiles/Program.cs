using System;
using System.IO;
using System.Text;

var frontends = new[] {"admin", "management", "registration", "shopping"};
var backends = new[] {"registration", "shopping"};
var services = new[] {"carts", "catalog", "media", "search", "stores", "accounts"};

string WriteCopyStatements(string type, string name)
{
    var t = type;
    var n = name;
    var u = char.ToUpper(n[0]) + n[1..];
    var s = new StringBuilder();
    s.AppendLine($"COPY \"src/{t}/{n}/{u}/{u}.csproj\" \"src/{t}/{n}/{u}/{u}.csproj\"");
    s.AppendLine($"COPY \"src/{t}/{n}/{u}.UnitTests/{u}.UnitTests.csproj\" \"src/{t}/{n}/{u}.UnitTests/{u}.UnitTests.csproj\"");
    s.AppendLine($"COPY \"src/{t}/{n}/{u}.FunctionalTests/{u}.FunctionalTests.csproj\" \"src/{t}/{n}/{u}.FunctionalTests/{u}.FunctionalTests.csproj\"");
    return s.ToString();
}

// Generates dockerfiles in a consistent mechanism to take best advantage of the caching
string GenerateDockerfile(string type, string name)
{
    var t = type;
    var n = name;
    var u = char.ToUpper(n[0]) + n[1..];
    var s = new StringBuilder();
    s.AppendLine("FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base");
    s.AppendLine("WORKDIR /app");
    s.AppendLine("EXPOSE 80");
    s.AppendLine("EXPOSE 81");
    
    s.AppendLine("");
    s.AppendLine("# add globalization support");
    s.AppendLine("RUN apk add --no-cache icu-libs");
    s.AppendLine("ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false");
    s.AppendLine("");
    s.AppendLine("# add diagnostic tools");
    s.AppendLine("RUN apk add --no-cache curl");
    
    s.AppendLine("");

    s.AppendLine("FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build");
    s.AppendLine("WORKDIR /work");
    s.AppendLine("");

    s.AppendLine("# Start build cache");
    s.AppendLine("COPY \"saas.sln\" \"saas.sln\"");
    s.AppendLine("");
    
    s.AppendLine("# frontends");
    foreach (var frontendName in frontends) { s.AppendLine(WriteCopyStatements("frontends", frontendName)); }
    
    s.AppendLine("# backends");
    foreach (var backendName in backends) { s.AppendLine(WriteCopyStatements("backends", backendName)); }
    
    s.AppendLine("# services");
    foreach (var serviceName in services) { s.AppendLine(WriteCopyStatements("services", serviceName)); }
    
    s.AppendLine("# Building blocks");
    s.AppendLine(string.Format("COPY \"src/{0}/{1}/{2}/{2}.csproj\" \"src/{0}/{1}/{2}/{2}.csproj\"", "building_blocks", "domain", "BuildingBlocks.Domain.DDD"));
    s.AppendLine(string.Format("COPY \"src/{0}/{1}/{2}/{2}.csproj\" \"src/{0}/{1}/{2}/{2}.csproj\"", "building_blocks", "application", "BuildingBlocks.Application"));
    s.AppendLine(string.Format("COPY \"src/{0}/{1}/{2}/{2}.csproj\" \"src/{0}/{1}/{2}/{2}.csproj\"", "building_blocks", "infrastructure", "BuildingBlocks.Infrastructure"));
    s.AppendLine(string.Format("COPY \"src/{0}/{1}/{2}/{2}.csproj\" \"src/{0}/{1}/{2}/{2}.csproj\"", "building_blocks", "infrastructure", "BuildingBlocks.Infrastructure.UnitTests"));
    
    s.AppendLine("# Messages");
    s.AppendLine(string.Format("COPY \"src/{0}/{1}/{2}/{2}.csproj\" \"src/{0}/{1}/{2}/{2}.csproj\"", "services", "carts", "Carts.Messages"));
    s.AppendLine(string.Format("COPY \"src/{0}/{1}/{2}/{2}.csproj\" \"src/{0}/{1}/{2}/{2}.csproj\"", "services", "catalog", "Catalog.Messages"));
    s.AppendLine(string.Format("COPY \"src/{0}/{1}/{2}/{2}.csproj\" \"src/{0}/{1}/{2}/{2}.csproj\"", "services", "media", "Media.Messages"));
    s.AppendLine(string.Format("COPY \"src/{0}/{1}/{2}/{2}.csproj\" \"src/{0}/{1}/{2}/{2}.csproj\"", "services", "search", "Search.Messages"));
    s.AppendLine(string.Format("COPY \"src/{0}/{1}/{2}/{2}.csproj\" \"src/{0}/{1}/{2}/{2}.csproj\"", "services", "stores", "Stores.Messages"));
    s.AppendLine(string.Format("COPY \"src/{0}/{1}/{2}/{2}.csproj\" \"src/{0}/{1}/{2}/{2}.csproj\"", "services", "accounts", "Accounts.Messages"));
    
    s.AppendLine("# others");
    s.AppendLine(string.Format("COPY \"{0}/{1}/{1}.csproj\" \"{0}/{1}/{1}.csproj\"", "tests", "SystemTests"));
    s.AppendLine(string.Format("COPY \"{0}/{1}/{1}.csproj\" \"{0}/{1}/{1}.csproj\"", "tools", "GenerateDockerFiles"));
    s.AppendLine("# End build cache");

    s.AppendLine("");
    s.AppendLine("RUN dotnet restore \"saas.sln\"");

    s.AppendLine("COPY . .");
    s.AppendLine($"WORKDIR /work/src/{t}/{n}/{u}");
    s.AppendLine("RUN dotnet publish --no-restore -c Release -o /app");
    s.AppendLine("");

    s.AppendLine("FROM build as unit_tests");
    s.AppendLine($"WORKDIR /work/src/{t}/{n}/{u}.UnitTests");
    s.AppendLine("");

    s.AppendLine("FROM build as functional_tests");
    s.AppendLine($"WORKDIR /work/src/{t}/{n}/{u}.FunctionalTests");
    s.AppendLine("");

    s.AppendLine("FROM build AS publish");
    s.AppendLine("");

    s.AppendLine("FROM base AS final");
    s.AppendLine("WORKDIR /app");
    s.AppendLine("COPY --from=publish /app .");
    s.AppendLine($"ENTRYPOINT [\"dotnet\", \"{u}.dll\"]");
    return s.ToString();
}

void SaveDockerFile(string type, string name, string content)
{
    var nameUpper = char.ToUpper(name[0]) + name[1..];
    string dir = Directory.GetParent(Environment.CurrentDirectory)
        .Parent // bin
        .Parent // Project Dir
        .Parent // tools
        .Parent // root
        .FullName;
    var file = dir + $"\\src\\{type}\\{name}\\{nameUpper}\\Dockerfile";
    File.WriteAllText(file, content);
    Console.WriteLine($"written to {file}");
}

void Generate(string[] names, string type)
{
    foreach (var name in names)
    {
        var dockerfile = GenerateDockerfile(type, name);
        SaveDockerFile(type, name, dockerfile);
    }
}

Generate(frontends, "frontends");
Generate(backends, "backends");
Generate(services, "services");
