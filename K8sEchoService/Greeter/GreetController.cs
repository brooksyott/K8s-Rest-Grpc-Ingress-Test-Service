using Microsoft.AspNetCore.Mvc;

namespace K8sEchoService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GreetController : ControllerBase
    {
        [HttpGet("{name}")]
        public IActionResult GetGreeting(string name)
        {
            return Ok($"Hello {name} from REST API");
        }
    }
}