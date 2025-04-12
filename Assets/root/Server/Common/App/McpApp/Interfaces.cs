#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.AspNetCore.SignalR.Client;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IMcpPlugin : IConnection, IDisposable
    {
        IRemoteServer? RemoteServer { get; }
        IMcpRunner McpRunner { get; }
    }
    public interface IConnection
    {
        HubConnectionState ConnectionState { get; }
        Task<bool> Connect(CancellationToken cancellationToken = default);
        Task Disconnect(CancellationToken cancellationToken = default);
    }
    public interface IMcpRunner : IToolRunner, IResourceRunner, IDisposable
    {
        bool HasTool(string name);
        bool HasResource(string name);
    }
    public interface IRemoteServer : IConnection, IToolResponseSender, IResourceResponseSender, IDisposable
    {
        Task<IResponseData<string>> UpdateTools(CancellationToken cancellationToken = default);
        Task<IResponseData<string>> UpdateResources(CancellationToken cancellationToken = default);
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

    // -----------------------------------------------------------------

    public interface IToolResponseSender
    {
        Task<IResponseData<string>> RespondOnCallTool(IResponseData<IResponseCallTool> data, CancellationToken cancellationToken = default);
        Task<IResponseData<string>> RespondOnListTool(IResponseData<List<IResponseListTool>> data, CancellationToken cancellationToken = default);
    }

    public interface IResourceResponseSender
    {
        Task<IResponseData<string>> RespondOnResourceContent(IResponseData<List<IResponseResourceContent>> data, CancellationToken cancellationToken = default);
        Task<IResponseData<string>> RespondOnListResources(IResponseData<List<IResponseListResource>> data, CancellationToken cancellationToken = default);
        Task<IResponseData<string>> RespondOnResourceTemplates(IResponseData<List<IResponseResourceTemplate>> data, CancellationToken cancellationToken = default);
    }

    // -----------------------------------------------------------------

    public interface IToolResponseReceiver
    {
        Task RespondOnCallTool(IResponseData<IResponseCallTool> data, CancellationToken cancellationToken = default);
        Task RespondOnListTool(IResponseData<List<IResponseListTool>> data, CancellationToken cancellationToken = default);
    }

    public interface IResourceResponseReceiver
    {
        Task RespondOnResourceContent(IResponseData<List<IResponseResourceContent>> data, CancellationToken cancellationToken = default);
        Task RespondOnListResources(IResponseData<List<IResponseListResource>> data, CancellationToken cancellationToken = default);
        Task RespondOnListResourceTemplates(IResponseData<List<IResponseResourceTemplate>> data, CancellationToken cancellationToken = default);
    }
}