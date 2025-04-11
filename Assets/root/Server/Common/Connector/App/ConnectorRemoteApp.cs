#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class ConnectorRemoteApp : IConnectorRemoteApp
    {
        public Task<IResponseCallTool> RunCallTool(IRequestCallTool data, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<IResponseListResource>> RunListResources(IRequestListResources data, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<IResponseListTool>> RunListTool(IRequestListTool data, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<IResponseResourceContent>> RunResourceContent(IRequestResourceContent data, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<IResponseResourceTemplate>> RunResourceTemplates(IRequestListResourceTemplates data, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}