#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Text.Json.Serialization;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class RequestData : IRequestData
    {
        public RequestCommand? Command { get; set; }
        public RequestResourceContent? Resource { get; set; }
        public RequestListResources? ListResources { get; set; }
        public RequestListResourceTemplates? ListResourceTemplates { get; set; }
        public RequestNotification? Notification { get; set; }

        [JsonIgnore]
        IRequestCommand? IRequestData.Command => Command;

        [JsonIgnore]
        IRequestResourceContent? IRequestData.Resource => Resource;

        [JsonIgnore]
        IRequestListResources? IRequestData.ListResources => ListResources;

        [JsonIgnore]
        IRequestListResourceTemplates? IRequestData.ListResourceTemplates => ListResourceTemplates;

        [JsonIgnore]
        IRequestNotification? IRequestData.Notification => Notification;

        public RequestData() { }
        public RequestData(RequestCommand command) : this()
        {
            Command = command;
        }
        public RequestData(RequestResourceContent resource) : this()
        {
            Resource = resource;
        }
        public RequestData(RequestListResources listResources) : this()
        {
            ListResources = listResources;
        }
        public RequestData(RequestListResourceTemplates listResourceTemplates) : this()
        {
            ListResourceTemplates = listResourceTemplates;
        }
        public RequestData(RequestNotification notification) : this()
        {
            Notification = notification;
        }

        public void Dispose()
        {
            Command?.Dispose();
            Resource?.Dispose();
            ListResources?.Dispose();
            ListResourceTemplates?.Dispose();
            Notification?.Dispose();
        }
        ~RequestData() => Dispose();
    }
}