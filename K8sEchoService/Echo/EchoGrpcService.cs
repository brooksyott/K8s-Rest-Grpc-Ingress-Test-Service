using Grpc.Core;

namespace K8sEchoService;

using System.Collections;
using K8sEchoService;
using K8sEchoService.Kubernetes;

public class EchoGrpcService : EchoGrpc.EchoGrpcBase
{
    private readonly ILogger<EchoGrpcService> _logger;
    public EchoGrpcService(ILogger<EchoGrpcService> logger)
    {
        _logger = logger;
    }

    public override Task<EchoGrpcResponse> Echo(EchoGrpcRequest request, ServerCallContext context)
    {
        _logger.LogInformation("********  In Echo Grpc Response  ********");

        // EchoRequestBody echoRequestBody = new EchoRequestBody();
        // echoRequestBody.Body = requestBody;


        var responseDetails = new EchoGrpcResponse();

        responseDetails.RemoteIpAddress = context.Peer;

        responseDetails.PodInformation = new K8sPodInformation
        {
            NodeName = GetEnvironmentVariable(KubernetesConstants.NodeNameEnvVarName),
            PodName = GetEnvironmentVariable(KubernetesConstants.PodNameEnvVarName),
            PodNamespace = GetEnvironmentVariable(KubernetesConstants.PodNamespaceEnvVarName),
            PodIp = GetEnvironmentVariable(KubernetesConstants.PodIpEnvVarName)
        };

        responseDetails.RequestBody = new EchoGrpcRequest
        {
            EnvironmentVariables = request.EnvironmentVariables
        };

        responseDetails.HostName = Environment.MachineName;
        responseDetails.Method = context.Method.ToUpper();
        responseDetails.Path = "n/a";
        responseDetails.Host = context.Host.ToString();


        var headers = context.RequestHeaders.ToDictionary(h => h.Key, h => h.Value.ToString());
        foreach (var header in headers)
        {
            responseDetails.AllHeaders.Add(header.Key, header.Value);
        }


        if (request.EnvironmentVariables == true)
        {
            var envVariables = Environment.GetEnvironmentVariables()
                                        .Cast<DictionaryEntry>()
                                        .ToDictionary(e => e.Key.ToString(), e => e.Value.ToString());

            foreach (var env in envVariables)
            {
                responseDetails.EnvironmentVariables.Add(env.Key, env.Value);
            }
        }

        return Task.FromResult(responseDetails);
    }

    string GetEnvironmentVariable(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (value == null)
        {
            return "";
        }
        return value;
    }
}
