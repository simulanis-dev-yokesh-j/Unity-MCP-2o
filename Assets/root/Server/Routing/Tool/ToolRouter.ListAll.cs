using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data;
using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public static partial class ToolRouter
    {
        public static async Task<ListToolsResult> ListAll(RequestContext<ListToolsRequestParams> request, CancellationToken cancellationToken)
        {
            var connector = Connector.Instance;
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

            return new ListToolsResult()
            {
                Tools = response.Value
                    .Where(x => x != null)
                    .Select(x => x!.ToTool())
                    .ToList()
            };
        }
    }
}