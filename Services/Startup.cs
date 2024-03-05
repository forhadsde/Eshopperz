using Microsoft.Extensions.Caching.StackExchangeRedis;

static void ConfigureServices(IServiceCollection services)
{
    // Other service configurations
    
    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "your-redis-connection-string";
        options.InstanceName = "your-instance-name";
    });

    // Other service configurations
}
