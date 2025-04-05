#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;

namespace com.IvanMurzak.UnityMCP.Common.Data
{
    public class NotificationData : INotificationData
    {
        public string? Name { get; set; }
        public IDictionary<string, object?>? Parameters { get; set; } = new Dictionary<string, object?>();

        public void Dispose()
        {
            Parameters?.Clear();
        }
        ~NotificationData() => Dispose();
    }
}