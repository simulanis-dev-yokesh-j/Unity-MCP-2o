#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Text.Json.Serialization;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class RequestData : IRequestData
    {
        public CommandData? Command { get; set; }
        public RequestResourceData? Resource { get; set; }
        public NotificationData? Notification { get; set; }

        [JsonIgnore]
        ICommandData? IRequestData.Command => Command;

        [JsonIgnore]
        IRequestResourceData? IRequestData.Resource => Resource;

        [JsonIgnore]
        INotificationData? IRequestData.Notification => Notification;

        public RequestData() { }
        public RequestData(CommandData command) : this()
        {
            Command = command;
        }
        public RequestData(RequestResourceData resource) : this()
        {
            Resource = resource;
        }
        public RequestData(NotificationData notification) : this()
        {
            Notification = notification;
        }

        public void Dispose()
        {
            Command?.Dispose();
            Notification?.Dispose();
        }
        ~RequestData() => Dispose();
    }
}