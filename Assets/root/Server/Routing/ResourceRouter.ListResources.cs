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

            var requestData = new RequestData(new RequestListResources());

            var resource = await connector.Send(requestData, cancellationToken: cancellationToken);
            if (resource == null)
                return new ListResourcesResult().SetError("[Error] Resource is null");

            if (!resource.IsSuccess)
                return new ListResourcesResult().SetError(resource.Message ?? "[Error] Unknown error");

            if (resource.ListResources == null)
                return new ListResourcesResult().SetError("[Error] ListResources data is null");

            return new ListResourcesResult()
            {
                Resources = resource.ListResources
                    .Where(x => x != null)
                    .Select(x => x!.ToResource())
                    .ToList()
            };
        }
    }
}