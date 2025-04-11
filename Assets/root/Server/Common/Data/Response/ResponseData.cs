#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class ResponseData<T> : IResponseData<T>
    {
        public bool IsError { get; set; }
        public string? Message { get; set; }
        public T? Value { get; set; }

        public ResponseData() { }
        public ResponseData(bool isError)
        {
            IsError = isError;
        }

        public static ResponseData<T> Success(string? message = null) => new(isError: false)
        {
            Message = message
        };
        public static ResponseData<T> Error(string? message = null) => new(isError: true)
        {
            Message = "[Error] " + message
        };
    }
}