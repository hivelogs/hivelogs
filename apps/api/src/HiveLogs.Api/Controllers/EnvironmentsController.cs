using HiveLogs.Api.Infrastructure.ResultMapping;
using HiveLogs.Application.Environments;
using HiveLogs.Application.Environments.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HiveLogs.Api.Controllers;

[ApiController]
[Route("organizations/{organizationId:guid}/applications/{applicationId:guid}/environments")]
public sealed class EnvironmentsController : ControllerBase
{
    private readonly IEnvironmentService _environmentService;

    public EnvironmentsController(IEnvironmentService environmentService) =>
        _environmentService = environmentService;

    [HttpPost]
    public async Task<IActionResult> Create(
        Guid organizationId,
        Guid applicationId,
        [FromBody] CreateEnvironmentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _environmentService.CreateAsync(
            organizationId,
            applicationId,
            request,
            cancellationToken);

        return result.ToCreatedAtActionResult(
            HttpContext,
            nameof(GetById),
            response => new
            {
                organizationId,
                applicationId,
                environmentId = response.Id
            });
    }

    [HttpGet]
    public async Task<IActionResult> List(
        Guid organizationId,
        Guid applicationId,
        CancellationToken cancellationToken)
    {
        var result = await _environmentService.ListByApplicationAsync(
            organizationId,
            applicationId,
            cancellationToken);

        return result.ToActionResult(HttpContext);
    }

    [HttpGet("{environmentId:guid}")]
    public async Task<IActionResult> GetById(
        Guid organizationId,
        Guid applicationId,
        Guid environmentId,
        CancellationToken cancellationToken)
    {
        var result = await _environmentService.GetByIdAsync(
            organizationId,
            applicationId,
            environmentId,
            cancellationToken);

        return result.ToActionResult(HttpContext);
    }
}
