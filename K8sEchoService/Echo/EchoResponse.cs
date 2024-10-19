namespace K8sEchoService.Echo;

using K8sEchoService.Configuration;
using K8sEchoService.Kubernetes;

public class EchoResponse
{
    public string HostName { get; set; }
    public string RemoteIpAddress { get; set; }
    public string Method { get; set; }
    public string Host { get; set; }
    public string Path { get; set; }
    public string Query { get; set; }
    public EchoRequestBody RequestBody { get; set; }

    public GeneralConfig GeneralConfig { get; set; }
    public PodInformation PodInformation { get; set; }
    public Dictionary<string, string> Headers { get; set; }
    public Dictionary<string, string> EnvironmentVariables { get; set; }

    // Other properties...
}

public class EchoRequestBody
{
    public string Body { get; set; }
}