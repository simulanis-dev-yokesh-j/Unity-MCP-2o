using System;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    sealed class PromptArgumentAttribute : Attribute
    {
        public string? Name { get; set; }

        public PromptArgumentAttribute() { }
    }
}