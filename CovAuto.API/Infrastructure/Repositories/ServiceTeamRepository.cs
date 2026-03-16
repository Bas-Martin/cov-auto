using CovAuto.API.Application.Interfaces;
using CovAuto.API.Domain.Entities;
using CovAuto.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CovAuto.API.Infrastructure.Repositories;

/// <summary>
/// Repository voor serviceteams. Handelt alle database-queries voor ServiceTeam af.
/// </summary>
public class ServiceTeamRepository : IServiceTeamRepository
{
    private readonly AppDbContext _context;

    public ServiceTeamRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ServiceTeam>> GetAllWithMembersAsync()
    {
        return await _context.ServiceTeams
            .Include(t => t.Members)
            .ToListAsync();
    }

    public async Task<ServiceTeam?> GetByIdWithMembersAsync(int id)
    {
        return await _context.ServiceTeams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<ServiceTeam?> GetByIdAsync(int id)
    {
        return await _context.ServiceTeams.FindAsync(id);
    }
}
