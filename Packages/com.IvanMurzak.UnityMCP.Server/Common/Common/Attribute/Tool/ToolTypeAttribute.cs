using System;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    [AttributeUsage(AttributeTargets.Class)]
    sealed class ToolTypeAttribute : Attribute
    {
        public string? Path { get; set; }

        public ToolTypeAttribute() { }
    }
}