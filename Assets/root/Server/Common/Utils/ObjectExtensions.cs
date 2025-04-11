#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System.Threading.Tasks;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static class ObjectExtensions
    {
        public static Task<T> TaskFromResult<T>(this T response)
            => Task.FromResult(response);
    }
}