builder.Services.AddReverseProxy()
    .LoadFromMemory(
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
        new[]
        {
            new ClusterConfig
            {
                ClusterId = "account_cluster",
                LoadBalancingPolicy = "RoundRobin",   // 🔹 Forzar balanceo
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "instance1", new DestinationConfig { Address = "https://account-service.onrender.com/" } },
                    { "instance2", new DestinationConfig { Address = "https://account-replica.onrender.com/" } }
                }
            },
            new ClusterConfig
            {
                ClusterId = "balanceo_cluster",
                LoadBalancingPolicy = "RoundRobin",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "instance1", new DestinationConfig { Address = "https://account-service.onrender.com/" } },
                    { "instance2", new DestinationConfig { Address = "https://account-replica.onrender.com/" } }
                }
            },
            new ClusterConfig
            {
                ClusterId = "payment_cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "payment1", new DestinationConfig { Address = "https://payment-service.onrender.com/" } }
                }
            }
        });
