namespace com.IvanMurzak.UnityMCP.Common.API
{
    public partial class Connector
    {
        public enum Status
        {
            Disconnected = 0,
            Connecting = 1,
            Connected = 2,
            Disconnecting = 3,
            Error = 4
        }
    }
}