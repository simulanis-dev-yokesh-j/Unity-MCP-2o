using System.Net;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public class ConnectorConfig
    {
        public IPAddress IPAddress { get; set; } = IPAddress.Loopback;
        public int PortServer { get; set; } = 60600;
        public int PortUnity { get; set; } = 60606;
        public Connector.ConnectionRole ConnectionType { get; set; } = Connector.ConnectionRole.Unity;

        public override string ToString()
            => $"IPAddress: {IPAddress}, PortServer: {PortServer}, PortUnity: {PortUnity} ConnectionType: {ConnectionType}";
    }
}