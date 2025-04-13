
using System;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using R3;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public interface ILocalServer : IToolResponseReceiver, IResourceResponseReceiver, IDisposable
    {
        Observable<Unit> OnListToolUpdated { get; }
        Observable<Unit> OnListResourcesUpdated { get; }

        Task<IResponseData<string>> SetOnListToolsUpdated(string data, CancellationToken cancellationToken = default);
        Task<IResponseData<string>> SetOnListResourcesUpdated(string data, CancellationToken cancellationToken = default);
    }

    public interface IToolResponseReceiver
    {
        // Task RespondOnCallTool(IResponseData<IResponseCallTool> data, CancellationToken cancellationToken = default);
        // Task RespondOnListTool(IResponseData<List<IResponseListTool>> data, CancellationToken cancellationToken = default);
    }

    public interface IResourceResponseReceiver
    {
        // Task RespondOnResourceContent(IResponseData<List<IResponseResourceContent>> data, CancellationToken cancellationToken = default);
        // Task RespondOnListResources(IResponseData<List<IResponseListResource>> data, CancellationToken cancellationToken = default);
        // Task RespondOnListResourceTemplates(IResponseData<List<IResponseResourceTemplate>> data, CancellationToken cancellationToken = default);
    }
}