FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

COPY ["./src/", "./src/src/"]
COPY NuGet.config ./src
COPY Directory.Build.props ./src
WORKDIR /src
RUN dotnet restore --configfile "./NuGet.config" "./src/UI/WebApp/PizzaStore.WebApp/PizzaStore.WebApp.csproj"
RUN dotnet build "./src/UI/WebApp/PizzaStore.WebApp/PizzaStore.WebApp.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./src/UI/WebApp/PizzaStore.WebApp/PizzaStore.WebApp.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PizzaStore.WebApp.dll"]
