using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Webapp_Dieselskandal.Data;
using Webapp_Dieselskandal.DTOs;

namespace Webapp_Dieselskandal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Dependancy Injection
    private readonly AppDbContext _context;             // Datenbankzugriff aus Program.cs
    private readonly IConfiguration _configuration;     // appsettings.json

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto) // Wartet auf POST unter api/auth/login
    {   // IActionResult für HTTP Response, RequestBody wird zu LoginDTO umgewandelt
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);
        // Sucht ersten Eintrag mit dieser Email, Problem bei doppelten Einträgen?

        if (user == null)
            return Unauthorized("Kein Account unter dieser Email vorhanden");

        var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Models.User>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        
        // Abgleich PW mit Datenbank, aktuell kein tieferes System mit X Versuchen
        if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
            return Unauthorized("Ungültiges Passwort");

        var token = GenerateJwtToken(user);
        
        return Ok(new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            Vorname = user.Vorname,
            Nachname = user.Nachname
        });
    }

    private string GenerateJwtToken(Models.User user)
    {
        // Meinen Key umwandeln und Signatur bestimmen
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Infos im Token
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.Vorname)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(_configuration["Jwt:ExpiresInMinutes"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}