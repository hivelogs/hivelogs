using HiveLogs.Api.Infrastructure.ResultMapping;
using HiveLogs.Application.Organizations;
using HiveLogs.Application.Organizations.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HiveLogs.Api.Controllers;

[ApiController]
[Route("organizations")]
public sealed class OrganizationsController : ControllerBase
{
    private readonly IOrganizationService _organizationService;

    public OrganizationsController(IOrganizationService organizationService) =>
        _organizationService = organizationService;

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrganizationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _organizationService.CreateAsync(request, cancellationToken);
        return result.ToCreatedAtActionResult(
            HttpContext,
            nameof(GetById),
            response => new { organizationId = response.Id });
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var result = await _organizationService.ListAsync(cancellationToken);
        return result.ToActionResult(HttpContext);
    }

    [HttpGet("{organizationId:guid}")]
    public async Task<IActionResult> GetById(
        Guid organizationId,
        CancellationToken cancellationToken)
    {
        var result = await _organizationService.GetByIdAsync(organizationId, cancellationToken);
        return result.ToActionResult(HttpContext);
    }
}
