#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public interface IRequestData : IDisposable
    {
        IRequestCommand? Command { get; }
        IRequestResourceContent? Resource { get; }
        IRequestListResources? ListResources { get; }
        IRequestListResourceTemplates? ListResourceTemplates { get; }
        IRequestNotification? Notification { get; }
    }
}