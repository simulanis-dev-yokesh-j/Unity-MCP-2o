#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IRpcRouter : IDisposable
    {
        void SetConnection(HubConnection hubConnection);
    }
}