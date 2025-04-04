namespace com.IvanMurzak.UnityMCP.Common
{
    internal static class Consts
    {
#if UNITY_EDITOR
        public static string LogTag => $"<color=#B4FF32>[AI]</color>";
#else
        public static string LogTag => "[AI]"
#endif
    }
}