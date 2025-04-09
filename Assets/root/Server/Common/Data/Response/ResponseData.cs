#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Linq;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class ResponseData : IResponseData
    {
        public bool IsSuccess { get; set; } = false;
        public string? Message { get; set; }
        public ResponseResourceContent[]? ResourceContents { get; set; }
        public ResponseListResource[]? ListResources { get; set; }
        public ResponseResourceTemplate[]? ListResourceTemplates { get; set; }

        IResponseResourceContent[]? IResponseData.ResourceContents => ResourceContents;
        IResponseListResource[]? IResponseData.ListResources => ListResources;
        IResponseResourceTemplate[]? IResponseData.ListResourceTemplates => ListResourceTemplates;

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
        public static ResponseData CreateResourceContents(string? message, IResponseResourceContent[] resourceContents) => new(isSuccess: true)
        {
            Message = message,
            ResourceContents = resourceContents.Cast<ResponseResourceContent>().ToArray()
        };
        public static ResponseData CreateListResources(string? message, IResponseListResource[] listResources) => new(isSuccess: true)
        {
            Message = message,
            ListResources = listResources.Cast<ResponseListResource>().ToArray()
        };
        public static ResponseData CreateListResourceTemplates(string? message, IResponseResourceTemplate[] resourceTemplates) => new(isSuccess: true)
        {
            Message = message,
            ListResourceTemplates = resourceTemplates.Cast<ResponseResourceTemplate>().ToArray()
        };
    }
}