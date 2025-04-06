#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public class Tool : ITool
    {
        public virtual string Class => GetType().FullName;
        // public string Description { get; set; } = string.Empty;
        // public object? Target { get; set; } = null!;
        // public List<object> Parameters { get; set; } = new List<object>();

        public virtual void Dispose()
        {

        }
    }
}