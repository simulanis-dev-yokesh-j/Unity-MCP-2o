using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data;
using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;
using NLog;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public static partial class ToolRouter
    {
        public static async Task<ListToolsResult> ListAll(RequestContext<ListToolsRequestParams> request, CancellationToken cancellationToken)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Trace("ListAll called");

            var connector = McpPlugin.Instance;
            if (connector == null)
                return new ListToolsResult().SetError("[Error] Connector is null");

            var remoteApp = connector.RemoteApp;
            if (remoteApp == null)
                return new ListToolsResult().SetError("[Error] Remote App is null");

            var requestData = new RequestListTool();

            var response = await remoteApp.RunListTool(requestData, cancellationToken: cancellationToken);
            if (response == null)
                return new ListToolsResult().SetError("[Error] Resource is null");

            if (response.IsError)
                return new ListToolsResult().SetError(response.Message ?? "[Error] Got an error during reading resources");

            if (response.Value == null)
                return new ListToolsResult().SetError("[Error] Resource value is null");

            var result = new ListToolsResult()
            {
                Tools = response.Value
                    .Where(x => x != null)
                    .Select(x => x!.ToTool())
                    .ToList()
            };

            logger.Trace("ListAll, result: {0}", JsonSerializer.Serialize(result));
            return result;
        }
    }
}