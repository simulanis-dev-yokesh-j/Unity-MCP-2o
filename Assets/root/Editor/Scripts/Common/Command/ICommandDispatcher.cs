using System;
namespace com.IvanMurzak.UnityMCP.Common.API
{
    public interface ICommandDispatcher : IDisposable
    {
        ICommandResponseData Dispatch(ICommandData data);
    }
}