using HiveLogs.Api.Infrastructure.ResultMapping;
using HiveLogs.Application.Setup;
using HiveLogs.Application.Setup.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HiveLogs.Api.Controllers;

[ApiController]
[Route("setup")]
public sealed class SetupController : ControllerBase
{
    private readonly ISetupService _setupService;

    public SetupController(ISetupService setupService) => _setupService = setupService;

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus(CancellationToken cancellationToken)
    {
        var result = await _setupService.GetStatusAsync(cancellationToken);
        return result.ToActionResult(HttpContext);
    }

    [HttpPost("initialize")]
    public async Task<IActionResult> Initialize(
        [FromBody] InitializeSetupRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _setupService.InitializeAsync(request, cancellationToken);

        if (result.IsSuccess)
            return StatusCode(StatusCodes.Status201Created, result.Value);

        return result.ToActionResult(HttpContext);
    }
}
