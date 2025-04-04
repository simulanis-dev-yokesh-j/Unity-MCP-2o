using UnityEngine;
using UnityEditor;
using com.IvanMurzak.UnityMCP.Common.API;

namespace com.IvanMurzak.UnityMCP.Editor
{
    [InitializeOnLoad]
    static class Startup
    {
        static Startup()
        {
            Debug.Log("[Unity MCP] Startup <color=red>---------------------------------------------</color>");

            new ConnectorBuilder()
                .WithCommandsFromAssembly()
                .Build();
        }
    }
}