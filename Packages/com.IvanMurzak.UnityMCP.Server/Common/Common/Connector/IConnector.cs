#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.UnityMCP.Common.Data;
using R3;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public interface IConnector : IDisposable
    {
        Connector.Status ReceiverStatus { get; }
        Connector.Status SenderStatus { get; }
        Observable<IDataPackage?> OnReceivedData { get; }
        void Connect();
        void Disconnect();
        Task<IResponseData?> Send(IDataPackage data, CancellationToken cancellationToken = default);
    }
    public interface IConnectorReceiver : IDisposable
    {
        Connector.Status GetStatus { get; }
        void Connect();
        void Disconnect();
        Observable<IDataPackage?> OnReceivedData { get; }
    }
    public interface IConnectorSender : IDisposable
    {
        Connector.Status GetStatus { get; }
        void Disconnect();
        Task<IResponseData?> Send(IDataPackage data, CancellationToken cancellationToken = default);
    }
}