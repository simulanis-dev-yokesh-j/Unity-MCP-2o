using System;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public interface IConnector : IDisposable
    {
        Connector.Status GetStatus { get; }
        void Connect();
        void Disconnect();
    }
}