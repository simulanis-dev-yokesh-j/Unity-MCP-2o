using System;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    sealed class ToolArgumentAttribute : Attribute
    {
        public string? Name { get; set; }

        public ToolArgumentAttribute() { }
    }
}