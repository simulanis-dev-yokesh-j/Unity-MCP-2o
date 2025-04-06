#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class ResponseData : IResponseData
    {
        public bool IsSuccess { get; set; }
        public string? SuccessMessage { get; set; } = null;
        public string? ErrorMessage { get; set; } = null;

        public static ResponseData Success(string? message = null) => new ResponseData()
        {
            IsSuccess = true,
            SuccessMessage = message
        };
        public static ResponseData Error(string? message = null) => new ResponseData()
        {
            IsSuccess = false,
            ErrorMessage = message
        };
    }
}