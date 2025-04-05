using System;
using com.IvanMurzak.UnityMCP.Common.Data;
namespace com.IvanMurzak.UnityMCP.Common
{
    public interface ICommandDispatcher : IDisposable
    {
        IResponseData Dispatch(ICommandData data);
    }
}