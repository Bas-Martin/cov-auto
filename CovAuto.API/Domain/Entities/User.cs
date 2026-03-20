using CovAuto.API.Domain.Enums;

namespace CovAuto.API.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    // Nullable for planners, required for monteurs
    public int? ServiceTeamId { get; set; }

    // Navigation property
    public ServiceTeam? ServiceTeam { get; set; }
}