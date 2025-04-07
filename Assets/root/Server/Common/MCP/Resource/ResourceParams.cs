#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common
{
    public class ResourceParams : IResourceParams
    {
        public string Route { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? MimeType { get; set; }
        public ICommand Command { get; set; }

        public ResourceParams(string route, string name, ICommand command, string? description = null, string? mimeType = null)
        {
            Route = route;
            Name = name;
            Command = command;
            Description = description;
            MimeType = mimeType;
        }
    }
}