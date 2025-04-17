using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Json.Converters;
using Debug = UnityEngine.Debug;
using Microsoft.Extensions.Logging;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    [InitializeOnLoad]
    static partial class Startup
    {


        static Startup()
        {
            McpPluginUnity.RegisterJsonConverters();
            McpPluginUnity.BuildAndStart();
            BuildServerIfNeeded(force: true);
        }
    }
}