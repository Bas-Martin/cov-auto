using CovAuto.API.Domain.Entities;

namespace CovAuto.API.Application.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Zoekt een gebruiker op gebruikersnaam, inclusief zijn serviceteam.
    /// </summary>
    Task<User?> GetByUsernameAsync(string username);
}
