#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common
{
  public static partial class Consts
  {
    public const string All = "*";
    public const string AllRecursive = "**";
    public const string PackageName = "com.ivanmurzak.unity.mcp";

    public static class Guid
    {
      public const string Zero = "00000000-0000-0000-0000-000000000000";
    }

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
        public static string Config(string executablePath, int port) => @"
{
  ""mcpServers"": {
    ""Unity-MCP"": {
      ""command"": ""{0}"",
      ""args"": [
        ""{1}""
      ]
    }
  }
}".Replace("{0}", executablePath).Replace("{1}", port.ToString());

      }
    }
  }
}