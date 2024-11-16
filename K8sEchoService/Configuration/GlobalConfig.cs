using System;
using System.Security.Cryptography;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace K8sEchoService.Configuration;

public class GeneralConfig {
    public SettingsData Settings { get; set; } = new SettingsData();
}

public class SettingsData
{
    public string Key1 { get; set; } = "default";
    public string Key2 { get; set; } = "default";
    public List<string> Items { get; set; } = new List<string>();
}

public static class GlobalConfig
{
    const string InContainerEnvVar = "DOTNET_RUNNING_IN_CONTAINER";
    public static string ConfigFileName { get; set; } = "generalConfig.yaml";

    private static string _currentHash = string.Empty;

    public static string ConfigDirectoryNoContainer { get; set; } = Environment.CurrentDirectory;
    // public static string ConfigDirectoryInContainer { get; set; } = "/app/config/..data"; // the ..data seems to be added by the container
    public static string ConfigDirectoryInContainer { get; set; } = "/app/config"; // the ..data seems to be added by the container

    static GeneralConfig _generalConfig = null;

    public static string GetConfigFile()
    {
        var inContainer = Environment.GetEnvironmentVariable(InContainerEnvVar);
        if (IsInContainer())    // If running in a container, use the container directory
        {
            return Path.Combine(ConfigDirectoryInContainer, ConfigFileName);
        }

        return Path.Combine(ConfigDirectoryNoContainer, ConfigFileName);
    }

    public static bool IsInContainer()
    {
        return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(InContainerEnvVar));
    }

    public static GeneralConfig GetConfig()
    {
        return _generalConfig;
    }

    public static bool HasChanged()
    {
        var configFile = GetConfigFile();
        string content = File.ReadAllText(configFile);
        using var md5 = MD5.Create();

        var hash = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(content)));
        if (hash != _currentHash)
        {
            _currentHash = hash;
            return true;
        }

        return false;
    }

    public static Boolean LoadConfig()
    {

        try{

            var configFile = GetConfigFile();
            if (!File.Exists(configFile))
            {
                if (_generalConfig == null)
                    _generalConfig = new GeneralConfig();

                _generalConfig.Settings = new SettingsData();
                return true;
            }

            string yamlContent = File.ReadAllText(configFile);

            // Parse the YAML content
            var deserializer = new DeserializerBuilder()
                                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                    .Build();

            _generalConfig = deserializer.Deserialize<GeneralConfig>(yamlContent);

            // Load the config
            return true;
        }
        catch {
            return false;
        }
    }
}
