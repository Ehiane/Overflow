var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var keycloak = builder.AddKeycloak("keycloak", 6001)
    .WithDataVolume("keycloak-data"); // Persist Keycloak data

var apiService = builder.AddProject<Projects.Overflow_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Overflow_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
