#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;

namespace com.IvanMurzak.Unity.MCP.Editor.Utils
{
    public static class TypeUtils
    {
        public static object CastTo(object obj, string typeFullName, out string error)
        {
            var type = Type.GetType(typeFullName);
            if (type == null)
            {
                error = $"[Error] Type '{typeFullName}' not found.";
                return default;
            }
            return CastTo(obj, type, out error);
        }

        public static T CastTo<T>(object obj, out string error)
            => CastTo(obj, typeof(T), out error) is T typedObj ? typedObj : default;

        public static object CastTo(object obj, Type type, out string error)
        {
            if (obj == null)
            {
                error = $"[Error] Object is null.";
                return default;
            }
            if (!type.IsAssignableFrom(obj.GetType()))
            {
                error = $"[Error] Type mismatch between '{type.FullName}' and '{obj.GetType().FullName}'.";
                return default;
            }

            error = string.Empty;
            return obj;
        }
    }
}