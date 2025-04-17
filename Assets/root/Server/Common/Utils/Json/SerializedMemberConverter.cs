#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;

namespace com.IvanMurzak.Unity.MCP.Common.Json
{
    public class SerializedMemberConverter : JsonConverter<SerializedMember>
    {
        public override SerializedMember? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var member = new SerializedMember();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read(); // Move to the value token

                    switch (propertyName)
                    {
                        case "name":
                            member.name = reader.GetString();
                            break;
                        case "type":
                            member.type = reader.GetString();
                            break;
                        case "json":
                            member.json = reader.GetString();
                            break;
                        default:
                            // Skip unknown properties
                            reader.Skip();
                            break;
                    }
                }
            }

            return member;
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
            writer.WriteString("name", value.name);

            // Write the "type" property
            writer.WriteString("type", value.type);

            writer.WritePropertyName("value");
            // Write the "json" property as an unwrapped object
            if (!string.IsNullOrEmpty(value.json))
            {
                try
                {
                    using (var jsonDocument = JsonDocument.Parse(value.json))
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