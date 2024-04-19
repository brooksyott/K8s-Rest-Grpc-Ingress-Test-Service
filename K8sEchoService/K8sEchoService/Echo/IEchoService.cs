namespace K8sEchoService.Echo;

public interface IEchoService
{
    Task<EchoResponse> Get(HttpRequest request, HttpContext context, string requestBody = null);
}