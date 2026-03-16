using CovAuto.API.Application.DTOs;
using CovAuto.API.Application.Interfaces;
using CovAuto.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CovAuto.API.Application.Services;

/// <summary>
/// Service voor het ophalen van serviceteams.
/// </summary>
public class ServiceTeamService : IServiceTeamService
{
    private readonly AppDbContext _context;

    public ServiceTeamService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ServiceTeamDto>> GetAllTeamsAsync()
    {
        var teams = await _context.ServiceTeams
            .Include(t => t.Members)
            .ToListAsync();

        return teams.Select(MapToDto);
    }

    public async Task<ServiceTeamDto?> GetTeamByIdAsync(int id)
    {
        var team = await _context.ServiceTeams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id);

        return team == null ? null : MapToDto(team);
    }

    // Mapping van entity naar DTO (eenvoudig manueel voor didactische duidelijkheid)
    private static ServiceTeamDto MapToDto(Domain.Entities.ServiceTeam team) =>
        new()
        {
            Id = team.Id,
            Name = team.Name,
            Description = team.Description,
            PlannerName = team.PlannerName,
            Members = team.Members.Select(m => new TeamMemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Username = m.Username
            }).ToList()
        };
}
