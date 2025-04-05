using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public static class CommandResponseDataExtension
    {
        public static ICommandResponseData Log(this ICommandResponseData response, ILogger logger)
        {
            if (response.IsSuccess)
                logger.LogInformation(response.SuccessMessage ?? "Command executed successfully.");
            else
                logger.LogError(response.ErrorMessage ?? "Command execution failed.");

            return response;
        }
    }
}