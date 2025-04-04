namespace com.IvanMurzak.UnityMCP.Common.API
{
    public class ConnectorConfig
    {
        public string Hostname { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 60606;

        public override string ToString()
            => $"Hostname: {Hostname}, Port: {Port}";
    }
}