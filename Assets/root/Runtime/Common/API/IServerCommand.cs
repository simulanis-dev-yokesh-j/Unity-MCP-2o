#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Threading.Tasks;
using com.IvanMurzak.UnityMCP.Common.Data;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public interface IServerCommand : IDisposable
    {
        string Class { get; }
        string Method { get; }
        Task<string> Execute(Action<ICommandData> configCommand);
    }
}