#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class ResponseResource : IResponseResource
    {
        public string Uri { get; set; } = string.Empty;
        public string? MimeType { get; set; }
        public string? Text { get; set; }
        public string? Blob { get; set; }

        public static ResponseResource CreateText(string uri, string text) => new ResponseResource()
        {
            Uri = uri,
            Text = text
        };
        public static ResponseResource CreateBlob(string uri, string blob) => new ResponseResource()
        {
            Uri = uri,
            Blob = blob
        };
    }
}