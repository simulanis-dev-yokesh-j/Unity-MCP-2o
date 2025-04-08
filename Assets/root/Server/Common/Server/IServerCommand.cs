#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;

namespace com.IvanMurzak.Unity.MCP.Common.Server
{
    public interface IServerCommand : IDisposable
    {
        string Class { get; }
        string? Method { get; }
        Task<string> Execute(Action<IRequestCommand> configCommand);
    }
}