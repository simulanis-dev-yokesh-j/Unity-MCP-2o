#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static class ObjectUtils
    {
        public static T[] MakeArray<T>(this T item) => new T[] { item };
    }
}