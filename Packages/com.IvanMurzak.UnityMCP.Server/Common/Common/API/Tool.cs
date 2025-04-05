using System.Collections.Generic;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public class Tool : ITool
    {
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public object? Target { get; set; } = null!;
        public List<object> Parameters { get; set; } = new List<object>();

        public void Dispose()
        {

        }
    }
}