using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Webapp_Dieselskandal.Data;

namespace Webapp_Dieselskandal.Tests;

public class TestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17")
        .WithDatabase("testdb")
        .WithUsername("postgres")
        .WithPassword("testpassword")
        .Build();
    
    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Echte DB Verbindung entfernen
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Testcontainer DB einsetzen
            services.AddDbContext<AppDbContext>(options =>
                NpgsqlDbContextOptionsBuilderExtensions.UseNpgsql(options, _postgres.GetConnectionString()));
        });
    }

    public new async Task DisposeAsync()
    {
        await _postgres.StopAsync();
    }
}