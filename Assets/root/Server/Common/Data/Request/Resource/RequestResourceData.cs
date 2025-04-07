#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class RequestResourceData : IRequestResourceData
    {
        public string? Uri { get; set; }

        public RequestResourceData() { }
        public RequestResourceData(string uri) : this()
        {
            Uri = uri;
        }

        public virtual void Dispose()
        {

        }
        ~RequestResourceData() => Dispose();
    }
}