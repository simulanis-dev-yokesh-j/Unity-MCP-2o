#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public interface IResponseResource
    {
        string Uri { get; set; }
        string? MimeType { get; set; }
        string? Text { get; set; }
        string? Blob { get; set; }
    }
}