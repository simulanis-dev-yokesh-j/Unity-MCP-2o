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