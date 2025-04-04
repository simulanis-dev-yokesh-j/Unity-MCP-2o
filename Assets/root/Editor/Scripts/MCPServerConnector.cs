using UnityEngine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using UnityEditor;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Options;
using System.Net;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;
using System.Collections.Generic;
using System;
using System.Net.Sockets;

namespace com.IvanMurzak.UnityMCP.Editor
{
    [InitializeOnLoad]
    public partial class MCPServerConnector : IDisposable
    {
        public const string Version = "0.1.0";

        TcpClient tcpClient;
        NetworkStream networkStream;

        public bool IsConnected => false; // runningServer?.Status == TaskStatus.Running;


        async void Start()
        {

        }
        void Stop()
        {

        }
        public void Dispose()
        {

        }
        ~MCPServerConnector() => Dispose();
    }
}