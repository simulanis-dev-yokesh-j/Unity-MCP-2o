#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace com.IvanMurzak.Unity.MCP.Common.Data.Utils
{
    [System.Serializable]
    public class SerializedMember
    {
        public string name = string.Empty;
        public string type = string.Empty;
        public string json = string.Empty;

        public SerializedMember[]? properties;

        public string Name
        {
            get => name;
            set => name = value;
        }
        public string Type
        {
            get => type;
            set => type = value;
        }
        public string Json
        {
            get => json;
            set => json = value;
        }
        public SerializedMember[]? Properties
        {
            get => properties;
            set => properties = value;
        }
        public SerializedMember() { }
        public SerializedMember(string name, string type, string json)
        {
            this.name = name;
            this.type = type;
            this.json = json;
        }
    }
}