#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.UnityMCP.Common.Data
{
    public interface IResponseData
    {
        bool IsSuccess { get; set; }
        string? SuccessMessage { get; set; }
        string? ErrorMessage { get; set; }
    }
}