
using System;
using com.IvanMurzak.Unity.MCP.Common;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public interface ILocalServer : IToolResponseReceiver, IResourceResponseReceiver, IDisposable
    {
    }
}