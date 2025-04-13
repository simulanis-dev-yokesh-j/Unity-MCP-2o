#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IRemoteServer : IConnection, IToolResponseSender, IResourceResponseSender, IDisposableAsync
    {
        Task<ResponseData<string>> NotifyAboutUpdatedTools(CancellationToken cancellationToken = default);
        Task<ResponseData<string>> NotifyAboutUpdatedResources(CancellationToken cancellationToken = default);
    }

    // -----------------------------------------------------------------

    public interface IToolResponseSender
    {
        // Task<IResponseData<string>> RespondOnCallTool(IResponseData<IResponseCallTool> data, CancellationToken cancellationToken = default);
        // Task<IResponseData<string>> RespondOnListTool(IResponseData<IResponseListTool[]> data, CancellationToken cancellationToken = default);
    }

    public interface IResourceResponseSender
    {
        // Task<IResponseData<string>> RespondOnResourceContent(IResponseData<List<IResponseResourceContent>> data, CancellationToken cancellationToken = default);
        // Task<IResponseData<string>> RespondOnListResources(IResponseData<List<IResponseListResource>> data, CancellationToken cancellationToken = default);
        // Task<IResponseData<string>> RespondOnResourceTemplates(IResponseData<List<IResponseResourceTemplate>> data, CancellationToken cancellationToken = default);
    }
}