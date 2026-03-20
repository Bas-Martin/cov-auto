using CovAuto.API.Application.DTOs;
using CovAuto.API.Application.Interfaces;
using CovAuto.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace CovAuto.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Inloggen en JWT token ophalen.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
            return Unauthorized(ApiResponse<string>.Fail("Ongeldige gebruikersnaam of wachtwoord."));

        return Ok(ApiResponse<LoginResponse>.Ok(result, "Succesvol ingelogd."));
    }
}