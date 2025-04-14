#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

using System.Linq;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static class StringUtils
    {
        public static string TrimPath(string path)
            => path?.TrimEnd('/')?.TrimStart('/');
        public static string Path_GetParentFolderPath(string path)
        {
            if (path == null)
                return null;
            var trimmedPath = path.TrimEnd('/');
            var lastSlashIndex = trimmedPath.LastIndexOf('/');
            return lastSlashIndex >= 0 ? trimmedPath.Substring(0, lastSlashIndex) : trimmedPath;
        }
        public static string Path_GetLastName(string path)
            => path?.TrimEnd('/')?.Split('/')?.Last();
    }
}