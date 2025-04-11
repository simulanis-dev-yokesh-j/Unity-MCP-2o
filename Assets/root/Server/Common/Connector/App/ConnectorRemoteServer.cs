#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Threading;
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public class ConnectorRemoteServer : IConnectorRemoteServer
    {
        public Task UpdateResources(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateTools(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}