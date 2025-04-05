#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using com.IvanMurzak.UnityMCP.Common.Data;
namespace com.IvanMurzak.UnityMCP.Common
{
    public interface ICommandDispatcher : IDisposable
    {
        IResponseData Dispatch(ICommandData data);
    }
}