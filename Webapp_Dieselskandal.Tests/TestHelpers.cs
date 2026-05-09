using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Webapp_Dieselskandal.Tests;

// Damit die Tests Tokens erstellen können
public static class TestHelpers
{
    public static async Task<string> LoginAndGetToken(HttpClient client, string email, string password)
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email,
            password
        });

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result?.Token ?? string.Empty;
    }

    public static void SetAuthHeader(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}

public record LoginResponse(string Token);