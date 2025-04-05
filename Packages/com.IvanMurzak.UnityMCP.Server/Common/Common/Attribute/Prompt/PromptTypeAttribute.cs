#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
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