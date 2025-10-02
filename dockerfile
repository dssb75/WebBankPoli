# Etapa de compilación: usa la imagen oficial de .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el código
COPY . .

# Restaurar dependencias y compilar ApiGateway
RUN dotnet restore "ApiGateway/ApiGateway.csproj"
RUN dotnet publish "ApiGateway/ApiGateway.csproj" -c Release -o /app/publish

# Etapa final: imagen ligera con ASP.NET Core 8 runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponer puerto 5000 (para local/dev)
EXPOSE 5000

# Importante para Render: que escuche en 0.0.0.0 y use la variable PORT
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

# Ejecutar la app
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
