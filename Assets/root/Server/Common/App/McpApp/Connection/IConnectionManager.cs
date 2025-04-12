#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IConnectionManager : IDisposable
    {
        HubConnectionState GetStatus { get; }
        Task InvokeAsync<TInput>(string methodName, TInput input, CancellationToken cancellationToken = default);
        Task<TResult> InvokeAsync<TInput, TResult>(string methodName, TInput input, CancellationToken cancellationToken = default);
        Task<bool> Connect(CancellationToken cancellationToken = default);
        void Disconnect();
    }
}