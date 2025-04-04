using System.Net;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public class ConnectorConfig
    {
        public IPAddress IPAddress { get; set; } = IPAddress.Loopback;
        public int Port { get; set; } = 60606;
        public Connector.ConnectionType ConnectionType { get; set; } = Connector.ConnectionType.Client;

        public override string ToString()
            => $"IPAddress: {IPAddress}, Port: {Port}, ConnectionType: {ConnectionType}";
    }
}