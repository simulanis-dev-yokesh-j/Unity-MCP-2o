#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class RequestListResources : IRequestListResources
    {
        public RequestListResources() { }

        public virtual void Dispose()
        {

        }
        ~RequestListResources() => Dispose();
    }
}