#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Threading.Tasks;
using R3;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public partial class McpPlugin : IMcpPlugin
    {
        // readonly static CompositeDisposable _disposables = new();
        readonly static ReactiveProperty<McpPlugin?> _instance = new(null);

        public static bool HasInstance => _instance.CurrentValue != null;
        public static IMcpPlugin? Instance => _instance.CurrentValue;

        public IDisposable OnInstanceCreated(Action<McpPlugin> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            return _instance
                .Where(x => x != null)
                .Subscribe(action);
            //.AddTo(_disposables);
        }
        public IDisposable OnInstanceDestroyed(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            return _instance
                .Where(x => x == null)
                .Subscribe(plugin => action());
            //.AddTo(_disposables);
        }

        public static Task StaticDisposeAsync()
            => _instance.CurrentValue?.DisposeAsync() ?? Task.CompletedTask;
    }
}