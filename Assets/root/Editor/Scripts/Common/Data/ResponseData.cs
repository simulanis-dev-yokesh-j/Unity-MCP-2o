namespace com.IvanMurzak.UnityMCP.Common.Data
{
    public class ResponseData : IResponseData
    {
        public bool IsSuccess { get; set; }
        public string? SuccessMessage { get; set; } = null;
        public string? ErrorMessage { get; set; } = null;

        public static ResponseData Success(string message = null) => new ResponseData()
        {
            IsSuccess = true,
            SuccessMessage = message
        };
        public static ResponseData Error(string message = null) => new ResponseData()
        {
            IsSuccess = false,
            ErrorMessage = message
        };
    }
}