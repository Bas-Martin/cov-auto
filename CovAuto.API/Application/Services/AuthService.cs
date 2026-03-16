using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CovAuto.API.Application.DTOs;
using CovAuto.API.Application.Interfaces;
using CovAuto.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CovAuto.API.Application.Services;

/// <summary>
/// Service voor authenticatie en JWT token generatie.
/// </summary>
public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        // Zoek de gebruiker op username
        var user = await _context.Users
            .Include(u => u.ServiceTeam)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
            return null;

        // Controleer het wachtwoord
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        // Genereer een JWT token
        var token = GenerateJwtToken(user);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            TeamId = user.ServiceTeamId
        };
    }

    private string GenerateJwtToken(Domain.Entities.User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Claims bevatten gebruikersinformatie die in het token wordt opgeslagen
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("teamId", user.ServiceTeamId?.ToString() ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
