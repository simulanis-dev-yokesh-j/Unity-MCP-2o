using System;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    [AttributeUsage(AttributeTargets.Class)]
    sealed class ResourcesTypeAttribute : Attribute
    {
        public string? Path { get; set; }

        public ResourcesTypeAttribute() { }
    }
}