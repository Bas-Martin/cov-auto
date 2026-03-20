using CovAuto.API.Application.QueryParameters;
using CovAuto.API.Common;
using CovAuto.API.Domain.Entities;

namespace CovAuto.API.Application.Interfaces;

public interface IWorkOrderRepository
{
    /// <summary>
    /// Geeft een gepagineerde lijst van werkorders terug, met optioneel filtering op team.
    /// Filtering, sorting en pagination worden toegepast via de query parameters.
    /// </summary>
    Task<PagedResult<WorkOrder>> GetPagedAsync(WorkOrderQueryParameters queryParams, int? teamIdFilter = null);

    /// <summary>
    /// Geeft één werkorder terug inclusief serviceteam, of null als de werkorder niet bestaat.
    /// </summary>
    Task<WorkOrder?> GetByIdAsync(int id);

    /// <summary>
    /// Voegt een nieuwe werkorder toe en geeft de opgeslagen werkorder terug inclusief serviceteam.
    /// </summary>
    Task<WorkOrder> AddAsync(WorkOrder workOrder);

    /// <summary>
    /// Geeft alle werkorders van een team in een bepaalde periode terug.
    /// </summary>
    Task<List<WorkOrder>> GetByTeamAndPeriodAsync(int teamId, DateTime from, DateTime to);
}
