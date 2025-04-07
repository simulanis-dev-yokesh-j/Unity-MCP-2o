#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class ResponseData : IResponseData
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; } = null;
        public ResponseResource?[]? Data { get; set; } = null;

        IResponseResource?[]? IResponseData.Data => Data;

        public static ResponseData Success(string? message = null) => new ResponseData()
        {
            IsSuccess = true,
            Message = message
        };
        public static ResponseData Error(string? message = null) => new ResponseData()
        {
            IsSuccess = false,
            Message = message
        };
    }
}