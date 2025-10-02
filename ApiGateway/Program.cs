using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Logging para ver a qué cluster se redirige
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Leer URLs desde variables de entorno (Render -> Settings -> Environment)
var accountServiceUrl = Environment.GetEnvironmentVariable("ACCOUNT_SERVICE_URL")
    ?? "https://account-service.onrender.com/";
var accountReplicaUrl = Environment.GetEnvironmentVariable("ACCOUNT_REPLICA_URL")
    ?? "https://account-replica.onrender.com/";
var paymentServiceUrl = Environment.GetEnvironmentVariable("PAYMENT_SERVICE_URL")
    ?? "https://payment-service.onrender.com/";

builder.Services.AddReverseProxy()
    .LoadFromMemory(
        // Rutas
        new[]
        {
            new RouteConfig
            {
                RouteId = "account_route",
                ClusterId = "account_cluster",
                Match = new() { Path = "/api/accounts/{**catch-all}" }
            },
            new RouteConfig
            {
                RouteId = "balanceo_route",
                ClusterId = "balanceo_cluster",
                Match = new() { Path = "/api/balanceo/{**catch-all}" }
            },
            new RouteConfig
            {
                RouteId = "payment_route",
                ClusterId = "payment_cluster",
                Match = new() { Path = "/api/payments/{**catch-all}" }
            }
        },
        // Clusters
        new[]
        {
            new ClusterConfig
            {
                ClusterId = "account_cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "instance1", new DestinationConfig { Address = accountServiceUrl } },
                    { "instance2", new DestinationConfig { Address = accountReplicaUrl } }
                }
            },
            new ClusterConfig
            {
                ClusterId = "balanceo_cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "instance1", new DestinationConfig { Address = accountServiceUrl } },
                    { "instance2", new DestinationConfig { Address = accountReplicaUrl } }
                }
            },
            new ClusterConfig
            {
                ClusterId = "payment_cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "payment1", new DestinationConfig { Address = paymentServiceUrl } }
                }
            }
        });

var app = builder.Build();
app.MapReverseProxy();

// Usa el puerto de Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");
