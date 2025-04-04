using System;
using System.Threading;
using System.Threading.Tasks;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public interface IConnector : IDisposable
    {
        Connector.Status GetStatus { get; }
        void Connect();
        Task Send(string message, CancellationToken cancellationToken = default);
        void Disconnect();
    }
}