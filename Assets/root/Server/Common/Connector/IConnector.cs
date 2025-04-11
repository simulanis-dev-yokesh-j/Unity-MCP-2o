#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.AspNetCore.SignalR.Client;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IConnector : IDisposable
    {
        HubConnectionState GetStatus { get; }
        IConnectorRemoteServer? Server { get; }
        IConnectorRemoteApp? App { get; }
        IConnectorLocalApp AppLocal { get; }
        void Connect();
        void Disconnect();
    }

    public interface IConnectorRemoteApp
    {
        Task<IResponseCallTool> RunCallTool(IRequestCallTool data, CancellationToken cancellationToken = default);
        Task<List<IResponseListTool>> RunListTool(IRequestListTool data, CancellationToken cancellationToken = default);

        Task<List<IResponseResourceContent>> RunResourceContent(IRequestResourceContent data, CancellationToken cancellationToken = default);
        Task<List<IResponseListResource>> RunListResources(IRequestListResources data, CancellationToken cancellationToken = default);
        Task<List<IResponseResourceTemplate>> RunResourceTemplates(IRequestListResourceTemplates data, CancellationToken cancellationToken = default);
    }
    public interface IConnectorLocalApp : IConnectorRemoteApp
    {
        bool HasTool(string name);
        bool HasResource(string name);
    }

    public interface IConnectorRemoteServer
    {
        Task UpdateTools(CancellationToken cancellationToken = default);
        Task UpdateResources(CancellationToken cancellationToken = default);
    }
}