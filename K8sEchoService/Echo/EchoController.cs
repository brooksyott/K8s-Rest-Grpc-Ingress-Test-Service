/// Purpose: Defines the EchoController class.
/// 
using Microsoft.AspNetCore.Mvc;

namespace K8sEchoService.Echo;

[ApiController]
[Route("/echo")]
public class EchoController : ControllerBase
{
    private readonly ILogger<EchoController> _logger;
    private readonly IEchoService _echoService;

    public EchoController(ILogger<EchoController> logger, IEchoService echoService)
    {
        _logger = logger;
        _echoService = echoService;
    }

    [HttpPost]
    [HttpPut]
    [HttpPatch]
    [HttpGet]
    [HttpDelete]
    public async Task<IActionResult> Echo()
    {
        using StreamReader reader = new(Request.Body, leaveOpen: false);
        var bodyAsString = await reader.ReadToEndAsync();

        EchoResponse echoRc = await _echoService.Get(Request, HttpContext, bodyAsString);
        return Ok(echoRc);
    }

    // private bool IsGrpcRequest(HttpContext context)
    // {
    //     // Check if the Content-Type is for gRPC and protocol is HTTP/2
    //     return context.Request.ContentType?.StartsWith("application/grpc") == true &&
    //            context.Request.Protocol == "HTTP/2";
    // }
}