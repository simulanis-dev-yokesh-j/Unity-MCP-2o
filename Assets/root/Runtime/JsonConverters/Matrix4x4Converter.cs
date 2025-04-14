using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP.Common.Json.Converters
{
    public class Matrix4x4Converter : JsonConverter<Matrix4x4>
    {
        public override Matrix4x4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            float[] elements = new float[16];
            int index = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return new Matrix4x4(
                        new Vector4(elements[0], elements[1], elements[2], elements[3]),
                        new Vector4(elements[4], elements[5], elements[6], elements[7]),
                        new Vector4(elements[8], elements[9], elements[10], elements[11]),
                        new Vector4(elements[12], elements[13], elements[14], elements[15])
                    );

                if (reader.TokenType == JsonTokenType.Number)
                {
                    elements[index++] = reader.GetSingle();
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Matrix4x4 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    writer.WriteNumber($"m{i}{j}", value[i, j]);
                }
            }
            writer.WriteEndObject();
        }
    }
}
