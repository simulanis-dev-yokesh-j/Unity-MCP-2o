#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;

namespace com.IvanMurzak.Unity.MCP.Common
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ResourceAttribute : Attribute
    {
        public string? Route { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? MimeType { get; set; }

        public ResourceAttribute() { }
    }
}