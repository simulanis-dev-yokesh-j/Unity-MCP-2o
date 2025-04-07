#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using com.IvanMurzak.Unity.MCP.Common.Data;
namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IResourceDispatcher : IDisposable
    {
        IResponseData Dispatch(IRequestResourceData data);
    }
}