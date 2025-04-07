using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Protocol.Types;
using ModelContextProtocol.Server;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public static partial class ResourceRouter
    {
        public static Task<ListResourceTemplatesResult> ListResourceTemplates(RequestContext<ListResourceTemplatesRequestParams> request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ListResourceTemplatesResult()
            {
                ResourceTemplates = new List<ResourceTemplate>()
                {
                    new ResourceTemplate()
                    {
                        UriTemplate = "gameObject://{path}",
                        Name = "GameObject",
                        Description = "GameObject template",
                        MimeType = "plain/text"
                    },
                    new ResourceTemplate()
                    {
                        UriTemplate = "component://{name}",
                        Name = "Component",
                        Description = "Component is attachable to GameObject C# class",
                        MimeType = "plain/text"
                    }
                }
            });
        }
    }
}