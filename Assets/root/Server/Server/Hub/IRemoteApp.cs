
using System;
using com.IvanMurzak.Unity.MCP.Common;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public interface IRemoteApp : IToolRunner, IResourceRunner, IDisposable
    {
    }
}