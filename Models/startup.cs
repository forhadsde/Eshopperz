using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // ...

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        // Add logging services
        services.AddLogging(builder =>
        {
            builder.AddSerilog(); // Use Serilog
        });

        // ...
    }
}