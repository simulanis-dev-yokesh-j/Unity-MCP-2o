using System.Collections.Generic;
using com.IvanMurzak.UnityMCP.Common.Data;

namespace com.IvanMurzak.UnityMCP.Common
{
    public static class NotificationDataExtensions
    {
        public static INotificationData SetName(this INotificationData data, string name)
        {
            data.Name = name;
            return data;
        }
        public static INotificationData SetOrAddParameter(this INotificationData data, string name, object? value)
        {
            data.Parameters ??= new Dictionary<string, object?>();
            data.Parameters[name] = value;
            return data;
        }
        public static IDataPackage Build(this INotificationData data) => new DataPackage()
        {
            Notification = data
        };
    }
}