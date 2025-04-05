using com.IvanMurzak.UnityMCP.Common;
using com.IvanMurzak.UnityMCP.Common.API;
using com.IvanMurzak.UnityMCP.Common.Data;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Threading.Tasks;

namespace com.IvanMurzak.UnityMCP.Server.API.Tool
{
    [McpServerToolType]
    public class EchoTool
    {
        public string Name => "EchoTool";

        [McpServerTool, Description("Echoes the message back to the client.")]
        public async Task<string?> Echo(string message)
        {
            var connector = Connector.Instance;
            if (connector == null)
                return null;

            var response = await connector.Send(new CommandData(Name)
                .SetOrAddParameter(nameof(message), message)
                .Build());

            return response.ToJson();
        }
    }
}