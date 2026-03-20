using CovAuto.API.Application.DTOs;

namespace CovAuto.API.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}