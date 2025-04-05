using System;
using System.Collections.Generic;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public interface ICommandData : IDisposable
    {
        string Name { get; }
        Dictionary<string, object?> Parameters { get; set; }
    }
}