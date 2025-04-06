#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Text.Json.Serialization;

namespace com.IvanMurzak.UnityMCP.Common.Data
{
    public class DataPackage : IDataPackage
    {
        public CommandData? Command { get; set; }
        public NotificationData? Notification { get; set; }

        [JsonIgnore]
        ICommandData? IDataPackage.Command => Command;

        [JsonIgnore]
        INotificationData? IDataPackage.Notification => Notification;

        public void Dispose()
        {
            Command?.Dispose();
            Notification?.Dispose();
        }
        ~DataPackage() => Dispose();
    }
}