namespace com.IvanMurzak.UnityMCP.Common.API
{
    public partial class Connector : IConnector
    {
        static Connector? instance;

        public static bool HasInstance => instance != null;
        public static Connector? Instance => instance;
    }
}