using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Webapp_Dieselskandal.Data;

namespace Webapp_Dieselskandal.Tests;

public class AuthControllerTests : IClassFixture<TestFactory>
{
    private readonly HttpClient _client;
    private readonly TestFactory _factory;

    public AuthControllerTests(TestFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private void SeedTestUser()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Webapp_Dieselskandal.Models.User>();
            var user = new Webapp_Dieselskandal.Models.User
            {
                Email = "test@test.de",
                Vorname = "Test",
                Nachname = "User"
            };
            user.PasswordHash = hasher.HashPassword(user, "Test1234!");
            context.Users.Add(user);
            context.SaveChanges();
        }
    }

    [Fact]
    public async Task LoginRichtig()
    {
        SeedTestUser();

        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "test@test.de",
            password = "Test1234!"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task LoginFalschesPw()
    {
        SeedTestUser();

        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "test@test.de",
            password = "643556365"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task LoginFalscheEmail()
    {
        SeedTestUser();

        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "nichtvorhanden@test.de",
            password = "Test1234!"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}