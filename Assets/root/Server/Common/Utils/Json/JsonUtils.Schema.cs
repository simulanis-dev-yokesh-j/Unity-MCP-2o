#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static partial class JsonUtils
    {
        public static JsonNode? GetSchema(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            var parameters = method.GetParameters();
            if (parameters.Length == 0)
                return null; // No parameters to generate schema for

            // Create a schema object manually
            var schema = new JsonObject
            {
                ["type"] = "object",
                ["properties"] = new JsonObject(),
                ["required"] = new JsonArray()
            };

            var properties = (JsonObject)schema["properties"]!;
            var required = (JsonArray)schema["required"]!;

            foreach (var parameter in parameters)
            {
                // Use JsonSchemaExporter to get the schema for each parameter type
                var parameterSchema = JsonSchemaExporter.GetJsonSchemaAsNode(jsonSerializerOptions, type: parameter.ParameterType);
                if (parameterSchema == null)
                    continue;

                properties[parameter.Name!] = parameterSchema;

                // Add to "required" if the parameter is not nullable
                if (!IsNullable(parameter.ParameterType))
                    required.Add(parameter.Name!);
            }
            return schema;
        }

        public static JsonElement? ToJsonElement(this JsonNode? node)
        {
            if (node == null)
                return null;

            // Convert JsonNode to JsonElement
            var jsonString = node.ToJsonString();

            // Parse the JSON string into a JsonElement
            using var document = JsonDocument.Parse(jsonString);
            return document.RootElement.Clone();
        }

        private static bool IsNullable(Type type)
        {
            if (!type.IsValueType)
                return true; // Reference types are nullable
            return Nullable.GetUnderlyingType(type) != null; // Nullable value types
        }
    }
}