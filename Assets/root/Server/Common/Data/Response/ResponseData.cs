#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class ResponseData : IResponseData
    {
        public bool IsSuccess { get; set; } = false;
        public string? Message { get; set; }
        public ResponseResourceContent[]? ResourceContents { get; set; }
        public ResponseListResources[]? ListResources { get; set; }
        public ResponseResourceTemplates[]? ListResourceTemplates { get; set; }

        IResponseResourceContent[]? IResponseData.ResourceContents => ResourceContents;
        IResponseListResources[]? IResponseData.ListResources => ListResources;
        IResponseListResourceTemplates[]? IResponseData.ListResourceTemplates => ListResourceTemplates;

        public ResponseData() { }
        public ResponseData(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public static ResponseData Success(string? message = null) => new(isSuccess: true)
        {
            Message = message
        };
        public static ResponseData Error(string? message = null) => new(isSuccess: false)
        {
            Message = "[Error] " + message
        };
        public static ResponseData CreateResources(string? message, ResponseResourceContent[]? resources) => new(isSuccess: true)
        {
            Message = message,
            ResourceContents = resources
        };
        public static ResponseData CreateListResources(string? message, ResponseResourceTemplates[]? resourcesTemplate) => new(isSuccess: true)
        {
            Message = message,
            ListResourceTemplates = resourcesTemplate
        };
        public static ResponseData CreateListResourceTemplates(string? message, ResponseResourceTemplates[]? resourcesTemplate) => new(isSuccess: true)
        {
            Message = message,
            ListResourceTemplates = resourcesTemplate
        };
    }
}