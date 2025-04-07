#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class NotificationData : INotificationData
    {
        public string? Path { get; set; }
        public string? Name { get; set; }
        public IDictionary<string, object?>? Parameters { get; set; } = new Dictionary<string, object?>();

        public NotificationData() { }
        public NotificationData(string path, string name) : this()
        {
            Path = path;
            Name = name;
        }

        public virtual void Dispose()
        {
            Parameters?.Clear();
        }
        ~NotificationData() => Dispose();
    }
}