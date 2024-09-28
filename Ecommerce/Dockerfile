# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY Ecommerce.sln .
COPY Ecommerce.Domain/Ecommerce.Domain.csproj Ecommerce.Domain/
COPY Ecommerce.Services/Ecommerce.Services.csproj Ecommerce.Services/
COPY Ecommerce.Controllers/Ecommerce.Controllers.csproj Ecommerce.Controllers/
COPY Ecommerce.Infrastructure/Ecommerce.Infrastructure.csproj Ecommerce.Infrastructure/
COPY Ecommerce.Tests/Ecommerce.Tests.csproj Ecommerce.Tests/

RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /app/Ecommerce.Infrastructure
RUN dotnet publish -c Release -o /app/publish

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENV ConnectionStrings__NeonDatabase="Host=ep-frosty-heart-a2hwa2gi.eu-central-1.aws.neon.tech;Database=ecommercedb;Username=ecommercedb_owner;Password=amikM5tl4reL;SSL Mode=Require;"

ENTRYPOINT ["dotnet", "Ecommerce.Infrastructure.dll"]