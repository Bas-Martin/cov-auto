using System.Security.Claims;
using CovAuto.API.Application.DTOs;
using CovAuto.API.Application.Interfaces;
using CovAuto.API.Application.QueryParameters;
using CovAuto.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovAuto.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class WorkOrdersController : ControllerBase
{
    private readonly IWorkOrderService _workOrderService;

    public WorkOrdersController(IWorkOrderService workOrderService)
    {
        _workOrderService = workOrderService;
    }

    /// <summary>
    /// Geeft werkorders terug met filtering, sorting en pagination.
    /// Planner ziet alle werkorders, monteur alleen zijn eigen team.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<WorkOrderDto>>), 200)]
    public async Task<IActionResult> GetWorkOrders([FromQuery] WorkOrderQueryParameters queryParams)
    {
        // Bepaal of de gebruiker beperkt moet worden tot zijn eigen team
        int? teamIdFilter = GetTeamIdFilter();

        var result = await _workOrderService.GetWorkOrdersAsync(queryParams, teamIdFilter);
        return Ok(ApiResponse<PagedResult<WorkOrderDto>>.Ok(result));
    }

    /// <summary>
    /// Geeft één werkorder terug. Monteur mag alleen werkorders van eigen team zien.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<WorkOrderDto>), 200)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetWorkOrder(int id)
    {
        var workOrder = await _workOrderService.GetWorkOrderByIdAsync(id);
        if (workOrder == null)
            return NotFound(ApiResponse<string>.Fail($"Werkorder {id} niet gevonden."));

        // Monteur mag alleen werkorders van eigen team zien
        var role = User.FindFirstValue(ClaimTypes.Role);
        if (role == "Monteur")
        {
            var teamIdClaim = User.FindFirstValue("teamId");
            if (!int.TryParse(teamIdClaim, out var userTeamId) || workOrder.ServiceTeamId != userTeamId)
                return StatusCode(403, ApiResponse<string>.Fail("U heeft geen toegang tot deze werkorder."));
        }

        return Ok(ApiResponse<WorkOrderDto>.Ok(workOrder));
    }

    /// <summary>
    /// Maakt een nieuwe werkorder aan. Alleen beschikbaar voor planners.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Planner")]
    [ProducesResponseType(typeof(ApiResponse<WorkOrderDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateWorkOrder([FromBody] CreateWorkOrderRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var workOrder = await _workOrderService.CreateWorkOrderAsync(request);
        return CreatedAtAction(nameof(GetWorkOrder), new { id = workOrder.Id },
            ApiResponse<WorkOrderDto>.Ok(workOrder, "Werkorder aangemaakt."));
    }

    // Hulpmethode: geeft team ID filter terug voor monteurs, null voor planners
    private int? GetTeamIdFilter()
    {
        var role = User.FindFirstValue(ClaimTypes.Role);
        if (role != "Monteur")
            return null;

        var teamIdClaim = User.FindFirstValue("teamId");
        return int.TryParse(teamIdClaim, out var teamId) ? teamId : null;
    }
}