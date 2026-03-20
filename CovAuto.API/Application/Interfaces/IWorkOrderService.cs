using CovAuto.API.Application.DTOs;
using CovAuto.API.Application.QueryParameters;
using CovAuto.API.Common;

namespace CovAuto.API.Application.Interfaces;

public interface IWorkOrderService
{
    Task<PagedResult<WorkOrderDto>> GetWorkOrdersAsync(WorkOrderQueryParameters queryParams, int? teamIdFilter = null);
    Task<WorkOrderDto?> GetWorkOrderByIdAsync(int id);
    Task<WorkOrderDto> CreateWorkOrderAsync(CreateWorkOrderRequest request);
}