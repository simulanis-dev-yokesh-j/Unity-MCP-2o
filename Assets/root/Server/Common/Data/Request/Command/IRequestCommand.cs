#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public interface IRequestCommand : IDisposable
    {
        string? Class { get; set; }
        string? Method { get; set; }
        IDictionary<string, object?>? Parameters { get; set; }
    }
}