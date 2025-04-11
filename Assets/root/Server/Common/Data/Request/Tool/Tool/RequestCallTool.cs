#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using System.Text.Json;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class RequestCallTool : IRequestCallTool
    {
        public string Name { get; set; } = string.Empty;
        // IDictionary<string, object?>? Parameters { get; set; }
        public Dictionary<string, JsonElement> Arguments { get; set; } = new();

        public RequestCallTool() { }
        public RequestCallTool(string name, Dictionary<string, JsonElement> arguments)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            Arguments = arguments ?? throw new System.ArgumentNullException(nameof(arguments));
        }
        // public RequestCallTool(string name, Dictionary<string, object?> parameters)
        // {
        //     Name = name;
        //     Arguments = new Dictionary<string, JsonElement>();
        //     foreach (var parameter in parameters)
        //     {
        //         Arguments.Add(parameter.Key, JsonSerializer.SerializeToElement(parameter.Value));
        //     }
        // }

        public virtual void Dispose()
        {
            Arguments?.Clear();
        }
        ~RequestCallTool() => Dispose();
    }
}