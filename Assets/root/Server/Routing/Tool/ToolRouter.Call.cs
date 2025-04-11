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
        public static async Task<CallToolResponse> Call(RequestContext<CallToolRequestParams> request, CancellationToken cancellationToken)
        {
            if (request == null)
                return new CallToolResponse().SetError("[Error] Request is null");

            if (request.Params == null)
                return new CallToolResponse().SetError("[Error] Request.Params is null");

            if (request.Params.Arguments == null)
                return new CallToolResponse().SetError("[Error] Request.Params.Arguments is null");

            var connector = Connector.Instance;
            if (connector == null)
                return new CallToolResponse().SetError("[Error] Connector is null");

            var app = connector.LocalApp.HasTool(request.Params.Name)
                ? connector.LocalApp as IToolRunner
                : connector.RemoteApp;

            if (app == null)
                return new CallToolResponse().SetError("[Error] Remote App is null");

            var requestData = new RequestCallTool(request.Params.Name, request.Params.Arguments);

            var response = await app.RunCallTool(requestData, cancellationToken);
            if (response == null)
                return new CallToolResponse().SetError("[Error] Resource is null");

            if (response.IsError)
                return new CallToolResponse().SetError(response.Message ?? "[Error] Got an error during reading resources");

            if (response.Value == null)
                return new CallToolResponse().SetError("[Error] Tool returned null value");

            return response.Value.ToCallToolRespose();
        }
    }
}