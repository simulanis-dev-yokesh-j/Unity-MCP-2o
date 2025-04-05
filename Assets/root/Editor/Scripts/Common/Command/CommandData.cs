using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public abstract class CommandData : ICommandData
    {
        public abstract string Path { get; }
        public abstract string Name { get; }

        public void Dispose()
        {
            // _logger.LogTrace("Dispose");
        }
        ~CommandData() => Dispose();
    }
}