namespace K8sEchoService.Echo;

using System.Collections;
using K8sEchoService.Configuration;
using K8sEchoService.Kubernetes;

public class EchoService : IEchoService
{
    private readonly ILogger<EchoService> _logger;

    public EchoService(ILogger<EchoService> logger)
    {
        _logger = logger;
    }

    public async Task<EchoResponse> Get(HttpRequest request, HttpContext context, string requestBody = null)
    {
        EchoRequestBody echoRequestBody = new EchoRequestBody();
        echoRequestBody.Body = requestBody;

        var requestDetails = new EchoResponse
        {
            Method = request.Method,
            Host = request.Host.ToString(),
            Path = request.Path,
            Query = request.QueryString.ToString(),
            HostName = Environment.MachineName,
            RemoteIpAddress = context.Connection.RemoteIpAddress?.ToString(),
            PodInformation = new PodInformation
            {
                NodeName = Environment.GetEnvironmentVariable(KubernetesConstants.NodeNameEnvVarName),
                PodName = Environment.GetEnvironmentVariable(KubernetesConstants.PodNameEnvVarName),
                PodNamespace = Environment.GetEnvironmentVariable(KubernetesConstants.PodNamespaceEnvVarName),
                PodIp = Environment.GetEnvironmentVariable(KubernetesConstants.PodIpEnvVarName)
            },
            Headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
            RequestBody = echoRequestBody,
            GeneralConfig = GlobalConfig.GetConfig(),
            EnvironmentVariables = Environment.GetEnvironmentVariables()
                                    .Cast<DictionaryEntry>()
                                    .ToDictionary(e => e.Key.ToString(), e => e.Value.ToString())
        };

        await Task.Delay(0);
        return requestDetails;
    }
}