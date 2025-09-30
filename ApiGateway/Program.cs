using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromMemory(
        new[]
        {
            // CRUD (balanceado entre original y réplica)
            new RouteConfig
            {
                RouteId = "account_route",
                ClusterId = "account_cluster",
                Match = new() { Path = "/api/accounts/{**catch-all}" }
            },
            // Balanceo de prueba (endpoint específico)
            new RouteConfig
            {
                RouteId = "balanceo_route",
                ClusterId = "balanceo_cluster",
                Match = new() { Path = "/api/balanceo/{**catch-all}" }
            }
        },
        new[]
        {
            // Balanceo para CRUD
            new ClusterConfig
            {
                ClusterId = "account_cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "instance1", new DestinationConfig { Address = "http://localhost:6001/" } },
                    { "instance2", new DestinationConfig { Address = "http://localhost:6003/" } }
                }
            },
            // Balanceo para endpoint de prueba
            new ClusterConfig
            {
                ClusterId = "balanceo_cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "instance1", new DestinationConfig { Address = "http://localhost:6001/" } },
                    { "instance2", new DestinationConfig { Address = "http://localhost:6003/" } }
                }
            }
        });

var app = builder.Build();
app.MapReverseProxy();
app.Run("http://localhost:5000");
