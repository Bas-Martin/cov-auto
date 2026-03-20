using CovAuto.API.Application.Interfaces;
using CovAuto.API.Domain.Entities;
using CovAuto.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CovAuto.API.Infrastructure.Repositories;

/// <summary>
/// Repository voor gebruikers. Handelt alle database-queries voor User af.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.ServiceTeam)
            .FirstOrDefaultAsync(u => u.Username == username);
    }
}