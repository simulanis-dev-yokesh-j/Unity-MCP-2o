using com.IvanMurzak.Unity.MCP.Common;
using Debug = UnityEngine.Debug;
using Microsoft.Extensions.Logging;
using UnityEditor;
using com.IvanMurzak.Unity.MCP.Common.Json.Converters;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    [InitializeOnLoad]
    static partial class Startup
    {
        const string PackageName = "com.ivanmurzak.unity.mcp";

        static Startup()
        {
            RegisterJsonConverters();
            BuildAndStart();
            BuildServerIfNeeded(force: true);
        }

        public static void BuildAndStart()
        {
            // if (McpPluginUnity.Instance.LogLevel.IsActive(LogLevel.Trace))
            // {
            //     var message = "<b><color=yellow>Startup</color></b>";
            //     Debug.Log($"{Consts.Log.Tag} {message} <color=orange>ಠ‿ಠ</color>");
            // }

            McpPlugin.StaticDisposeAsync();

            var mcpPlugin = new McpPluginBuilder()
                .WithAppFeatures()
                .WithConfig(config =>
                {
                    config.Endpoint = McpPluginUnity.Instance.Host;
                })
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders(); // 👈 Clears the default providers
                    loggingBuilder.AddProvider(new UnityLoggerProvider());
                    loggingBuilder.SetMinimumLevel(McpPluginUnity.Instance.LogLevel switch
                    {
                        MCP.LogLevel.Trace => Microsoft.Extensions.Logging.LogLevel.Trace,
                        MCP.LogLevel.Log => Microsoft.Extensions.Logging.LogLevel.Information,
                        MCP.LogLevel.Warning => Microsoft.Extensions.Logging.LogLevel.Warning,
                        MCP.LogLevel.Error => Microsoft.Extensions.Logging.LogLevel.Error,
                        MCP.LogLevel.Exception => Microsoft.Extensions.Logging.LogLevel.Critical,
                        _ => Microsoft.Extensions.Logging.LogLevel.Warning
                    });
                })
                .WithToolsFromAssembly(typeof(Startup).Assembly)
                .WithPromptsFromAssembly(typeof(Startup).Assembly)
                .WithResourcesFromAssembly(typeof(Startup).Assembly)
                .Build();

            if (McpPluginUnity.Instance.KeepConnected)
            {
                if (McpPluginUnity.Instance.LogLevel.IsActive(LogLevel.Log))
                {
                    var message = "<b><color=yellow>Connecting</color></b>";
                    Debug.Log($"{Consts.Log.Tag} {message} <color=orange>ಠ‿ಠ</color>");
                }
                mcpPlugin.Connect();
            }
        }

        public static void RegisterJsonConverters()
        {
            JsonUtils.AddConverter(new Color32Converter());
            JsonUtils.AddConverter(new ColorConverter());
            JsonUtils.AddConverter(new Matrix4x4Converter());
            JsonUtils.AddConverter(new QuaternionConverter());
            JsonUtils.AddConverter(new Vector2Converter());
            JsonUtils.AddConverter(new Vector2IntConverter());
            JsonUtils.AddConverter(new Vector3Converter());
            JsonUtils.AddConverter(new Vector3IntConverter());
            JsonUtils.AddConverter(new Vector4Converter());
        }
    }
}