using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data;
using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public static partial class ResourceRouter
    {
        public static async Task<ReadResourceResult> ReadResource(RequestContext<ReadResourceRequestParams> request, CancellationToken cancellationToken)
        {
            if (request?.Params?.Uri == null)
                return new ReadResourceResult().SetError("null", "[Error] Request or Uri is null");

            var connector = Connector.Instance;
            if (connector == null)
                return new ReadResourceResult().SetError(request.Params.Uri, "[Error] Connector is null");

            var remoteApp = connector.App;
            if (remoteApp == null)
                return new ReadResourceResult().SetError(request.Params.Uri, "[Error] Remote App is null");

            var requestData = new RequestResourceContent(request.Params.Uri);

            var resource = await remoteApp.RunResourceContent(requestData, cancellationToken: cancellationToken);
            if (resource == null)
                return new ReadResourceResult().SetError(request.Params.Uri, "[Error] Resource is null");

            return new ReadResourceResult()
            {
                Contents = resource
                    .Where(x => x != null)
                    .Where(x => x!.text != null || x!.blob != null)
                    .Select(x => x!.ToResourceContents())
                    .ToList()
            };
        }
    }
}