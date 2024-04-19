namespace K8sEchoService.Kubernetes;

public static class KubernetesConstants
{
    // Remote service URL to get it's health
    public const string NodeNameEnvVarName = "NODE_NAME";
    public const string PodNameEnvVarName = "POD_NAME";
    public const string PodNamespaceEnvVarName = "POD_NAMESPACE";
    public const string PodIpEnvVarName = "POD_IP";
}