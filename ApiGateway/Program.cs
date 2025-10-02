using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configuración de proxy
builder.Services.AddReverseProxy()
    .LoadFromMemory(
        // Rutas
        new[]
        {
            // CRUD en AccountService (balanceado entre original y réplica)
            new RouteConfig
            {
                RouteId = "account_route",
                ClusterId = "account_cluster",
                Match = new() { Path = "/api/accounts/{**catch-all}" }
            },
            // Balanceo de prueba
            new RouteConfig
            {
                RouteId = "balanceo_route",
                ClusterId = "balanceo_cluster",
                Match = new() { Path = "/api/balanceo/{**catch-all}" }
            },
            // CRUD en PaymentService
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
            // Balanceo CRUD (AccountService + Replica)
            new ClusterConfig
            {
                ClusterId = "account_cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "instance1", new DestinationConfig { Address = "https://account-service.onrender.com/" } },
                    { "instance2", new DestinationConfig { Address = "https://account-replica.onrender.com/" } }
                }
            },
            // Balanceo para endpoint de prueba
            new ClusterConfig
            {
                ClusterId = "balanceo_cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "instance1", new DestinationConfig { Address = "https://account-service.onrender.com/" } },
                    { "instance2", new DestinationConfig { Address = "https://account-replica.onrender.com/" } }
                }
            },
            // CRUD PaymentService
            new ClusterConfig
            {
                ClusterId = "payment_cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "payment1", new DestinationConfig { Address = "https://payment-service.onrender.com/" } }
                }
            }
        });

var app = builder.Build();
app.MapReverseProxy();

// Usa el puerto asignado por Render, o 5000 en local
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");
