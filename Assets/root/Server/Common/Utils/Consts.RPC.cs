#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common
{
    public static partial class Consts
    {
        public static partial class RPC
        {
            public const string RunCallTool = "/mcp/run-call-tool";
            public const string RunListTool = "/mcp/run-list-tool";
            public const string RunResourceContent = "/mcp/run-resource-content";
            public const string RunListResources = "/mcp/run-list-resources";
            public const string RunListResourceTemplates = "/mcp/run-list-resource-templates";


            public const string ResponseOnCallTool = "RespondOnCallTool";
            public const string ResponseOnListTool = "RespondOnListTool";
            public const string ResponseOnResourceContent = "RespondOnResourceContent";
            public const string ResponseOnListResources = "RespondOnListResources";
            public const string ResponseOnListResourceTemplates = "RespondOnResourceTemplates";
        }
    }
}