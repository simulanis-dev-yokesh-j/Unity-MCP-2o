#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;

namespace com.IvanMurzak.Unity.MCP.Common.Json
{
    public class SerializedMemberConverter : JsonConverter<SerializedMember>
    {
        public override SerializedMember Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Deserialization is not implemented for SerializedMember.");
        }

        public override void Write(Utf8JsonWriter writer, SerializedMember value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();

            // Write the "name" property
            writer.WriteString("name", value.Name);

            // Write the "type" property
            writer.WriteString("type", value.Type);

            writer.WritePropertyName("value");
            // Write the "json" property as an unwrapped object
            if (!string.IsNullOrEmpty(value.Json))
            {
                try
                {
                    using (var jsonDocument = JsonDocument.Parse(value.Json))
                    {
                        jsonDocument.RootElement.WriteTo(writer);
                    }
                }
                catch
                {
                    // If the Json property is invalid, write an empty object
                    writer.WriteStartObject();
                    writer.WriteEndObject();
                }
            }
            else
            {
                // If the Json property is null or empty, write an empty object
                writer.WriteStartObject();
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }
}