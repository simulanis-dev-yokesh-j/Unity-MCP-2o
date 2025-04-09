#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Text.Json.Serialization;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class RequestData : IRequestData
    {
        public RequestTool? Tool { get; set; }
        public RequestResourceContent? ResourceContents { get; set; }
        public RequestListResources? ListResources { get; set; }
        public RequestListResourceTemplates? ListResourceTemplates { get; set; }
        public RequestNotification? Notification { get; set; }

        [JsonIgnore]
        IRequestTool? IRequestData.Tool => Tool;

        [JsonIgnore]
        IRequestResourceContent? IRequestData.ResourceContents => ResourceContents;

        [JsonIgnore]
        IRequestListResources? IRequestData.ListResources => ListResources;

        [JsonIgnore]
        IRequestListResourceTemplates? IRequestData.ListResourceTemplates => ListResourceTemplates;

        [JsonIgnore]
        IRequestNotification? IRequestData.Notification => Notification;

        public RequestData() { }
        public RequestData(RequestTool command) : this()
        {
            Tool = command;
        }
        public RequestData(RequestResourceContent resource) : this()
        {
            ResourceContents = resource;
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
            Tool?.Dispose();
            ResourceContents?.Dispose();
            ListResources?.Dispose();
            ListResourceTemplates?.Dispose();
            Notification?.Dispose();
        }
        ~RequestData() => Dispose();
    }
}