#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Threading;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common.Data;

namespace com.IvanMurzak.Unity.MCP.Common.Server
{
    public abstract class ServerCommand : IServerCommand
    {
        public virtual string Class => GetType().FullName ?? "UnknownClass";
        public virtual int Retry => 3;
        public virtual string? Method => null;

        protected CancellationTokenSource? _cancellationTokenSource { get; private set; } = new();

        public Task<string> Execute(Action<ICommandData> configCommand)
            => Execute(Method, configCommand);

        public async Task<string> Execute(string? method, Action<ICommandData> configCommand)
        {
            if (_cancellationTokenSource == null)
                return "[Error] Command already executed. Please create a new instance of the command.";

            var connector = Connector.Instance;
            if (connector == null)
                return "[Error] No connection with Unity established. Please try to establish connection first and try again.";

            var finalMethod = method ?? Method;
            if (string.IsNullOrEmpty(finalMethod))
                return "[Error] Method name is not specified. Please specify a method name and try again.";

            var commandData = new CommandData(Class, finalMethod);
            try
            {
                configCommand.Invoke(commandData);
            }
            catch (Exception ex)
            {
                return $"[Error] Failed to configure command: {ex.Message}";
            }
            var requestData = commandData.BuildRequest();

            var response = await connector.Send(requestData, Retry, _cancellationTokenSource.Token);
            if (response == null)
                return "[Error] No response from Unity. Please check the connection.";

            return response.ToJson();
        }

        public virtual void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}