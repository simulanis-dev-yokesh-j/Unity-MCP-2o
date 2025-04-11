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
        public static async Task<ListResourcesResult> ListResources(RequestContext<ListResourcesRequestParams> request, CancellationToken cancellationToken)
        {
            var connector = Connector.Instance;
            if (connector == null)
                return new ListResourcesResult().SetError("[Error] Connector is null");

            var remoteApp = connector.App;
            if (remoteApp == null)
                return new ListResourcesResult().SetError("[Error] Remote App is null");

            var requestData = new RequestListResources(request?.Params?.Cursor);

            var resource = await remoteApp.RunListResources(requestData, cancellationToken: cancellationToken);
            if (resource == null)
                return new ListResourcesResult().SetError("[Error] Resource is null");

            return new ListResourcesResult()
            {
                Resources = resource
                    .Where(x => x != null)
                    .Select(x => x!.ToResource())
                    .ToList()
            };
        }
    }
}