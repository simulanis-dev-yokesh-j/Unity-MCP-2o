namespace com.IvanMurzak.UnityMCP.Common.API
{
    public class CommandResponseData : ICommandResponseData
    {
        public bool IsSuccess { get; set; }
        public string? SuccessMessage { get; set; } = null;
        public string? ErrorMessage { get; set; } = null;

        public static CommandResponseData Success(string message = null) => new CommandResponseData()
        {
            IsSuccess = true,
            SuccessMessage = message
        };
        public static CommandResponseData Error(string message = null) => new CommandResponseData()
        {
            IsSuccess = false,
            ErrorMessage = message
        };
    }
}