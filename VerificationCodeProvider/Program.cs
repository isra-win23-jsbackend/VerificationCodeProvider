using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using VerificationCodeProvider.Data.Contexts;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<DataContext>(x => x.UseSqlServer(Environment.GetEnvironmentVariable("SqlServer")));
    })
    .Build();


//servicebus funkar Addsingleton och database funkar AddScoped och
//den här koden måste göras för att få service bus funka.
//det är trick för att göra Add-Migration 

using (var scoped = host.Services.CreateScope())
{
    try
    {
        var context = scoped.ServiceProvider.GetRequiredService<DataContext>();
        var migration = context.Database.GetPendingMigrations();
        if (migration != null && migration.Any())
        {
            context.Database.Migrate();
        }
    }
    catch  (Exception ex)
    {
        Debug.WriteLine($"ERROR : CreateScope Program.cs :: {ex.Message}");
    }

}

host.Run();
