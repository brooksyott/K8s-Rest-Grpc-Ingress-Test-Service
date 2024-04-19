namespace K8sEchoService.Kubernetes;

using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("/health")]
public class ProbeController : ControllerBase
{
    private readonly ILogger<ProbeController> _logger;

    public ProbeController(ILogger<ProbeController> logger)
    {
        _logger = logger;
    }

    [HttpGet("live")]
    public async Task<IActionResult> LivenessProbe()
    {
        await Task.Delay(1);
        return StatusCode((int)HttpStatusCode.OK);
    }

    [HttpGet("ready")]
    public async Task<IActionResult> ReadinessProbe()
    {
        await Task.Delay(1);
        return StatusCode((int)HttpStatusCode.OK);
    }
}

