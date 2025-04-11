#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public interface IRequestCallTool : IDisposable
    {
        string Name { get; set; }
        // IDictionary<string, object?>? Parameters { get; set; }
        Dictionary<string, JsonElement> Arguments { get; set; }
    }
}