using System;
using System.Collections.Generic;

namespace com.IvanMurzak.UnityMCP.Common.Data
{
    public interface ICommandData : IDisposable
    {
        string Name { get; }
        Dictionary<string, object?> Parameters { get; set; }
    }
}