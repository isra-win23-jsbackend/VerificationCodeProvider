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
//den h�r koden m�ste g�ras f�r att f� service bus funka.
//det �r trick f�r att g�ra Add-Migration 

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
