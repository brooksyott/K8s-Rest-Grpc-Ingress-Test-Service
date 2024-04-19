namespace K8sEchoService.Kubernetes;
public class PodInformation
{
    public string NodeName { get; set; }
    public string PodName { get; set; }
    public string PodNamespace { get; set; }
    public string PodIp { get; set; }
}