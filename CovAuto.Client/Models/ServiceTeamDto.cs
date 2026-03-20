namespace CovAuto.Client.Models;

public class ServiceTeamDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PlannerName { get; set; } = string.Empty;
    public List<TeamMemberDto> Members { get; set; } = new();
}

public class TeamMemberDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}
