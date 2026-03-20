using System.Security.Claims;
using CovAuto.API.Application.DTOs;
using CovAuto.API.Application.Interfaces;
using CovAuto.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovAuto.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TeamsController : ControllerBase
{
    private readonly IServiceTeamService _teamService;

    public TeamsController(IServiceTeamService teamService)
    {
        _teamService = teamService;
    }

    /// <summary>
    /// Geeft alle teams terug. Alleen beschikbaar voor planners.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Planner")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ServiceTeamDto>>), 200)]
    public async Task<IActionResult> GetAllTeams()
    {
        var teams = await _teamService.GetAllTeamsAsync();
        return Ok(ApiResponse<IEnumerable<ServiceTeamDto>>.Ok(teams));
    }

    /// <summary>
    /// Geeft één team terug. Planner mag elk team ophalen, monteur alleen zijn eigen team.
    /// </summary>
    [HttpGet("{teamId:int}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceTeamDto>), 200)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetTeam(int teamId)
    {
        // Controleer of de gebruiker een monteur is en of het om zijn eigen team gaat
        var role = User.FindFirstValue(ClaimTypes.Role);
        if (role == "Monteur")
        {
            var teamIdClaim = User.FindFirstValue("teamId");
            if (!int.TryParse(teamIdClaim, out var userTeamId) || userTeamId != teamId)
                return StatusCode(403, ApiResponse<string>.Fail("U heeft geen toegang tot dit team."));
        }

        var team = await _teamService.GetTeamByIdAsync(teamId);
        if (team == null)
            return NotFound(ApiResponse<string>.Fail($"Team {teamId} niet gevonden."));

        return Ok(ApiResponse<ServiceTeamDto>.Ok(team));
    }
}