# Usa .NET 8 SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "AccountService/AccountService.csproj"
RUN dotnet publish "AccountService/AccountService.csproj" -c Release -o /app/publish

# Imagen final con runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
EXPOSE 5000
ENTRYPOINT ["dotnet", "AccountService.dll"]
