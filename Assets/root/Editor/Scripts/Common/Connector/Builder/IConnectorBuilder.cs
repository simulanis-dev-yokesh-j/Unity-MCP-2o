using System;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public interface IConnectorBuilder
    {
        IConnectorBuilder AddLogging(Action<ILoggingBuilder> loggingBuilder);
        IConnector Build();
    }
}