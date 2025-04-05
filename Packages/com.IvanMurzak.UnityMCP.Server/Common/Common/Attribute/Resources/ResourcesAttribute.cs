using System;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    [AttributeUsage(AttributeTargets.Method)]
    sealed class ResourcesAttribute : Attribute
    {
        public string? Name { get; set; }

        public ResourcesAttribute() { }
    }
}