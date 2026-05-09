using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Webapp_Dieselskandal.Data;
using Webapp_Dieselskandal.Models;

namespace Webapp_Dieselskandal.Tests;

public class AuftraegeControllerTests : IClassFixture<TestFactory>
{
    private readonly HttpClient _client;
    private readonly TestFactory _factory;

    public AuftraegeControllerTests(TestFactory factory)
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
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
            var user = new User
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
    public async Task GetOhneToken()
    {
        var response = await _client.GetAsync("/api/auftraege");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMitToken()
    {
        SeedTestUser();
        var token = await TestHelpers.LoginAndGetToken(_client, "test@test.de", "Test1234!");
        TestHelpers.SetAuthHeader(_client, token);

        var response = await _client.GetAsync("/api/auftraege");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateAuftragMitToken()
    {
        SeedTestUser();
        var token = await TestHelpers.LoginAndGetToken(_client, "test@test.de", "Test1234!");
        TestHelpers.SetAuthHeader(_client, token);

        var auftrag = new
        {
            hersteller = "VW",
            modell = "Golf",
            baujahr = 2020,
            fahrgestellnummer = "WVW12345678901234",
            kennzeichen = "KA-XX 123",
            kaufpreis = 25000,
            waehrung = 0,
            kaufdatum = "2020-06-15T00:00:00Z",
            haendler = "VW Autohaus",
            fahrzeugInBesitz = true,
            andereKanzleiBeauftragt = false,
            klageEingereicht = false,
            klageDatum = (string?)null
        };

        var response = await _client.PostAsJsonAsync("/api/auftraege", auftrag);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAuftragMitToken()
    {
        SeedTestUser();
        var token = await TestHelpers.LoginAndGetToken(_client, "test@test.de", "Test1234!");
        TestHelpers.SetAuthHeader(_client, token);

        // Erst Auftrag anlegen
        var auftrag = new
        {
            hersteller = "BMW",
            modell = "320d",
            baujahr = 2019,
            fahrgestellnummer = "WBA12345678901234",
            kennzeichen = "KA-YY 456",
            kaufpreis = 35000,
            waehrung = 0,
            kaufdatum = "2019-03-10T00:00:00Z",
            haendler = "BMW Autohaus",
            fahrzeugInBesitz = true,
            andereKanzleiBeauftragt = false,
            klageEingereicht = false,
            klageDatum = (string?)null
        };

        var createResponse = await _client.PostAsJsonAsync("/api/auftraege", auftrag);
        var id = await createResponse.Content.ReadFromJsonAsync<int>();

        // Dann löschen
        var deleteResponse = await _client.DeleteAsync($"/api/auftraege/{id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}