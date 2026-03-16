using CovAuto.API.Domain.Entities;

namespace CovAuto.API.Application.Interfaces;

public interface IServiceTeamRepository
{
    /// <summary>
    /// Geeft alle serviceteams terug inclusief teamleden.
    /// </summary>
    Task<IEnumerable<ServiceTeam>> GetAllWithMembersAsync();

    /// <summary>
    /// Geeft één serviceteam terug inclusief teamleden, of null als het team niet bestaat.
    /// </summary>
    Task<ServiceTeam?> GetByIdWithMembersAsync(int id);

    /// <summary>
    /// Geeft één serviceteam terug zonder teamleden, of null als het team niet bestaat.
    /// </summary>
    Task<ServiceTeam?> GetByIdAsync(int id);
}
