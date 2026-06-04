using HiveLogs.Api.Infrastructure.ResultMapping;
using HiveLogs.Application.Applications;
using HiveLogs.Application.Applications.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HiveLogs.Api.Controllers;

[ApiController]
[Route("organizations/{organizationId:guid}/applications")]
public sealed class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationsController(IApplicationService applicationService) =>
        _applicationService = applicationService;

    [HttpPost]
    public async Task<IActionResult> Create(
        Guid organizationId,
        [FromBody] CreateApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _applicationService.CreateAsync(organizationId, request, cancellationToken);
        return result.ToCreatedAtActionResult(
            HttpContext,
            nameof(GetById),
            response => new { organizationId, applicationId = response.Id });
    }

    [HttpGet]
    public async Task<IActionResult> List(
        Guid organizationId,
        CancellationToken cancellationToken)
    {
        var result = await _applicationService.ListByOrganizationAsync(organizationId, cancellationToken);
        return result.ToActionResult(HttpContext);
    }

    [HttpGet("{applicationId:guid}")]
    public async Task<IActionResult> GetById(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken)
    {
        var result = await _applicationService.GetByIdAsync(organizationId, applicationId, cancellationToken);
        return result.ToActionResult(HttpContext);
    }
}
