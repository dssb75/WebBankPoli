# Usa la imagen oficial de .NET 8 SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo
COPY . .

# Restaurar dependencias y compilar ApiGateway
RUN dotnet restore "ApiGateway/ApiGateway.csproj"
RUN dotnet publish "ApiGateway/ApiGateway.csproj" -c Release -o /app/publish

# Imagen final con ASP.NET Core 8 runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
