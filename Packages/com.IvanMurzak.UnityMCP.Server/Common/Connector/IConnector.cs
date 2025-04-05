using System;
using System.Threading;
using System.Threading.Tasks;
using R3;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public interface IConnector : IDisposable
    {
        Connector.Status ReceiverStatus { get; }
        Connector.Status SenderStatus { get; }
        void Connect();
        Task<string?> Send(string message, CancellationToken cancellationToken = default);
        void Disconnect();
    }
    public interface IConnectorReceiver : IDisposable
    {
        Connector.Status GetStatus { get; }
        void Connect();
        void Disconnect();
        Observable<string?> OnReceivedData { get; }
    }
    public interface IConnectorSender : IDisposable
    {
        Connector.Status GetStatus { get; }
        void Disconnect();
        Task<string?> Send(string message, CancellationToken cancellationToken = default);
    }
}