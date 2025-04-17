#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Collections.Generic;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;

namespace com.IvanMurzak.Unity.MCP.Common.Data.Unity
{
    [System.Serializable]
    public class ComponentDataLight
    {
        public string type { get; set; } = string.Empty;
        public Enabled isEnabled { get; set; }
        public int instanceId { get; set; }

        public ComponentDataLight() { }

        public enum Enabled
        {
            NA = -1,
            False = 0,
            True = 1
        }
    }
}