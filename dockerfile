# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copiar todo el código
COPY . .

# Restaurar dependencias y compilar ApiGateway
RUN dotnet restore "ApiGateway/ApiGateway.csproj"
RUN dotnet publish "ApiGateway/ApiGateway.csproj" -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copiar lo publicado desde la etapa build
COPY --from=build /app/publish .

# Comando de inicio
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
