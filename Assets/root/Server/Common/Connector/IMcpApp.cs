#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.AspNetCore.SignalR.Client;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IMcpApp : IDisposable
    {
        HubConnectionState GetStatus { get; }
        IRemoteServer? RemoteServer { get; }
        IRemoteApp? RemoteApp { get; }
        ILocalApp LocalApp { get; }
        Task Connect();
        void Disconnect();
    }
    public interface ILocalApp : IToolRunner, IResourceRunner, IDisposable
    {
        bool HasTool(string name);
        bool HasResource(string name);
    }
    public interface IRemoteApp : IToolRunner, IResourceRunner, IDisposable
    {
    }
    public interface IRemoteServer : IDisposable
    {
        Task UpdateTools(CancellationToken cancellationToken = default);
        Task UpdateResources(CancellationToken cancellationToken = default);
    }

    // -----------------------------------------------------------------

    public interface IToolRunner
    {
        Task<IResponseData<IResponseCallTool>> RunCallTool(IRequestCallTool data, CancellationToken cancellationToken = default);
        Task<IResponseData<List<IResponseListTool>>> RunListTool(IRequestListTool data, CancellationToken cancellationToken = default);
    }

    public interface IResourceRunner
    {
        Task<IResponseData<List<IResponseResourceContent>>> RunResourceContent(IRequestResourceContent data, CancellationToken cancellationToken = default);
        Task<IResponseData<List<IResponseListResource>>> RunListResources(IRequestListResources data, CancellationToken cancellationToken = default);
        Task<IResponseData<List<IResponseResourceTemplate>>> RunResourceTemplates(IRequestListResourceTemplates data, CancellationToken cancellationToken = default);
    }
}