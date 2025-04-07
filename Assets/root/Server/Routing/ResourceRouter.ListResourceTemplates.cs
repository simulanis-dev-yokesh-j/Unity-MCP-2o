using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
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
                        UriTemplate = Consts.Route.GameObject_CurrentScene,
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