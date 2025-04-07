#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using R3;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IConnector : IDisposable
    {
        Connector.Status ReceiverStatus { get; }
        Connector.Status SenderStatus { get; }
        Observable<IRequestData?> OnReceivedData { get; }
        void Connect();
        void Disconnect();
        Task<IResponseData?> Send(IRequestData data, int retry = 10, CancellationToken cancellationToken = default);
    }
    public interface IConnectorReceiver : IDisposable
    {
        Connector.Status GetStatus { get; }
        void Connect();
        void Disconnect();
        Observable<IRequestData?> OnReceivedData { get; }
    }
    public interface IConnectorSender : IDisposable
    {
        Connector.Status GetStatus { get; }
        void Disconnect();
        Task<IResponseData?> Send(IRequestData data, int retry = 10, CancellationToken cancellationToken = default);
    }
}