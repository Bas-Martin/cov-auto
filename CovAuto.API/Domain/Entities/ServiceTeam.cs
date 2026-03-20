namespace CovAuto.API.Domain.Entities;

public class ServiceTeam
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PlannerName { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<User> Members { get; set; } = new List<User>();
    public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}