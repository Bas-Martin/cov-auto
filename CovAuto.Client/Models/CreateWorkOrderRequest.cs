using System.ComponentModel.DataAnnotations;

namespace CovAuto.Client.Models;

public class CreateWorkOrderRequest
{
    [Required(ErrorMessage = "Titel is verplicht")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(0.5, 999, ErrorMessage = "Geschatte uren moeten tussen 0.5 en 999 liggen")]
    public double EstimatedHours { get; set; } = 1.0;

    public string Status { get; set; } = "Nieuw";

    public string Priority { get; set; } = "Normaal";

    public DateTime? ScheduledFor { get; set; }

    [Required(ErrorMessage = "Klantnaam is verplicht")]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adres is verplicht")]
    [MaxLength(300)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Team is verplicht")]
    public int ServiceTeamId { get; set; }
}
