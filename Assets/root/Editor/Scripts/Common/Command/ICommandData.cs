using System;
using System.Threading;
using System.Threading.Tasks;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public interface ICommandData : IDisposable
    {
        string Path { get; }
        string Name { get; }
        // Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}