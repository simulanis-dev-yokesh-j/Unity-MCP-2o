using System.Collections.Generic;

namespace com.IvanMurzak.UnityMCP.Common.Data
{
    public class CommandData : ICommandData
    {
        public string? Name { get; set; }
        public IDictionary<string, object?>? Parameters { get; set; } = new Dictionary<string, object?>();

        public CommandData() { }
        public CommandData(string name) : this() => Name = name;

        public void Dispose()
        {
            Parameters?.Clear();
        }
        ~CommandData() => Dispose();
    }
}