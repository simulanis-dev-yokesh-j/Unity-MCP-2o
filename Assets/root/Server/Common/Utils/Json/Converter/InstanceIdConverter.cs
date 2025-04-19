#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;

namespace com.IvanMurzak.Unity.MCP.Common.Json
{
    public class InstanceIdConverter : JsonConverter<InstanceId>
    {
        public override InstanceId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var instanceId = new InstanceId();

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
                        case "id":
                            instanceId.Id = reader.GetInt32();
                            break;
                        default:
                            // Skip unknown properties
                            reader.Skip();
                            break;
                    }
                }
            }

            return instanceId;
        }

        public override void Write(Utf8JsonWriter writer, InstanceId value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();

            // Write the "id" property
            writer.WritePropertyName("id");
            writer.WriteNumberValue(value.Id);

            writer.WriteEndObject();
        }
    }
}