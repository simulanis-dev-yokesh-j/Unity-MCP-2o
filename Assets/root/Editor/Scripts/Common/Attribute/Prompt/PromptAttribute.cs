using System;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    [AttributeUsage(AttributeTargets.Method)]
    sealed class PromptAttribute : Attribute
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public PromptAttribute() { }
    }
}