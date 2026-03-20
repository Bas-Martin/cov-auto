using CovAuto.API.Application.DTOs;

namespace CovAuto.API.Application.Interfaces;

public interface IServiceTeamService
{
    Task<IEnumerable<ServiceTeamDto>> GetAllTeamsAsync();
    Task<ServiceTeamDto?> GetTeamByIdAsync(int id);
}
