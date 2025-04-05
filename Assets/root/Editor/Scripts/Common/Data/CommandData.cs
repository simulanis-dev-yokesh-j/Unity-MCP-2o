using System.Collections.Generic;

namespace com.IvanMurzak.UnityMCP.Common.Data
{
    public class CommandData : ICommandData
    {
        public string Name { get; set; }
        public Dictionary<string, object?> Parameters { get; set; } = new();

        public void Dispose()
        {
            Parameters.Clear();
        }
        ~CommandData() => Dispose();
    }
}