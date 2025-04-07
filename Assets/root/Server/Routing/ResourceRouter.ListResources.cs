using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public static partial class ResourceRouter
    {
        public static Task<ListResourcesResult> ListResources(RequestContext<ListResourcesRequestParams> request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ListResourcesResult()
            {
                Resources = new List<Resource>()
                {
                    new Resource()
                    {
                        Uri = "scene://[active]/path/to/resource",
                        Name = "",
                        MimeType = "plain/text"
                    },
                    new Resource()
                    {
                        Uri = "component://path/to/resource",
                        Name = "Component",
                        MimeType = "plain/text"
                    }
                }
            });
        }
    }
}