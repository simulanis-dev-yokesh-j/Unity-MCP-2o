#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using R3;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public class LocalServer : BaseHub<LocalServer>, ILocalServer
    {
        readonly Subject<Unit> _onListToolUpdated = new();
        public Observable<Unit> OnListToolUpdated => _onListToolUpdated;

        readonly Subject<Unit> _onListResourcesUpdated = new();
        public Observable<Unit> OnListResourcesUpdated => _onListResourcesUpdated;

        public LocalServer(ILogger<LocalServer> logger, IHubContext<LocalServer> hubContext) : base(logger, hubContext)
        {
        }

        public new void Dispose()
        {
            base.Dispose();
        }

        public Task<IResponseData<string>> SetOnListToolsUpdated(CancellationToken cancellationToken = default)
        {
            _onListToolUpdated.OnNext(Unit.Default);
            return Task.FromResult<IResponseData<string>>(ResponseData<string>.Success(string.Empty, string.Empty));
        }

        public Task<IResponseData<string>> SetOnListResourcesUpdated(CancellationToken cancellationToken = default)
        {
            _onListResourcesUpdated.OnNext(Unit.Default);
            return Task.FromResult<IResponseData<string>>(ResponseData<string>.Success(string.Empty, string.Empty));
        }
    }
}