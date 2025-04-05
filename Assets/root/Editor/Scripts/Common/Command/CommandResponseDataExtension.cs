#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using com.IvanMurzak.UnityMCP.Common.Data;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.UnityMCP.Common
{
    public static class CommandResponseDataExtension
    {
        public static IResponseData Log(this IResponseData response, ILogger logger)
        {
            if (response.IsSuccess)
                logger.LogInformation(response.SuccessMessage ?? "Command executed successfully.");
            else
                logger.LogError(response.ErrorMessage ?? "Command execution failed.");

            return response;
        }
    }
}