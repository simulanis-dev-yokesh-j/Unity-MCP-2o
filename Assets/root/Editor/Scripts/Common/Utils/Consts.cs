namespace com.IvanMurzak.UnityMCP.Common
{
    internal static partial class Consts
    {
        public static partial class Command
        {
            public static partial class ResponseCode
            {
                public const string Success = "[Success]";
                public const string Error = "[Error]";
                public const string Cancel = "[Cancel]";
            }
        }
        public static partial class MCP_Client
        {
            public static partial class ClaudeDesktop
            {
                public const string Config =
                    "{\r\n" +
                    "  \"mcpServers\": {\r\n" +
                    "    \"Unity-MCP\": {\r\n" +
                    "      \"command\": \"{0}\",\r\n" +
                    "      \"args\": []\r\n" +
                    "    }\r\n" +
                    "  }\r\n" +
                    "}";
            }
        }
    }
}