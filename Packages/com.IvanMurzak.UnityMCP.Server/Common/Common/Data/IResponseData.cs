namespace com.IvanMurzak.UnityMCP.Common.Data
{
    public interface IResponseData
    {
        bool IsSuccess { get; set; }
        string? SuccessMessage { get; set; }
        string? ErrorMessage { get; set; }
    }
}