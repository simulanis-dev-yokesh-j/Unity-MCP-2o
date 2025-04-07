#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;

namespace com.IvanMurzak.Unity.MCP.Common.Data
{
    public class CommandData : ICommandData
    {
        public string? Class { get; set; }
        public string? Method { get; set; }
        public IDictionary<string, object?>? Parameters { get; set; } = new Dictionary<string, object?>();

        public CommandData() { }
        public CommandData(string path, string name) : this()
        {
            Class = path;
            Method = name;
        }

        public virtual void Dispose()
        {
            Parameters?.Clear();
        }
        ~CommandData() => Dispose();
    }
}