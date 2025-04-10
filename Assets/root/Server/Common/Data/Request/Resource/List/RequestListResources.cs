#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class RequestListResources : IRequestListResources
    {
        public string? Cursor { get; set; }

        public RequestListResources() { }
        public RequestListResources(string? cursor)
        {
            Cursor = cursor;
        }

        public virtual void Dispose()
        {

        }
        ~RequestListResources() => Dispose();
    }
}