#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.IvanMurzak.Unity.MCP.Common.Data.Utils;

namespace com.IvanMurzak.Unity.MCP.Common.Json
{
    public class InstanceIdConverter : JsonConverter<InstanceId>
    {
        public override InstanceId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Deserialization is not implemented for InstanceId.");
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