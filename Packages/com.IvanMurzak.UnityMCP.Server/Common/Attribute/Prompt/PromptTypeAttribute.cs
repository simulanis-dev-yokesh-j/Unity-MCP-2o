using System;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    [AttributeUsage(AttributeTargets.Class)]
    sealed class PromptTypeAttribute : Attribute
    {
        public string? Path { get; set; }

        public PromptTypeAttribute() { }
    }
}