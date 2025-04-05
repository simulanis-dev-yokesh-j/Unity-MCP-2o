using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public interface ICommandResponseData
    {
        bool IsSuccess { get; set; }
        string? SuccessMessage { get; set; }
        string? ErrorMessage { get; set; }
    }
}