#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common
{
    public interface IResourceParams
    {
        string Route { get; set; }
        string Name { get; set; }
        string? Description { get; set; }
        string? MimeType { get; set; }
        ICommand Command { get; set; }
    }
}