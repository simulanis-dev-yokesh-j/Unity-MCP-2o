#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
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