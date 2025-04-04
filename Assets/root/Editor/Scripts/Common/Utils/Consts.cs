namespace com.IvanMurzak.UnityMCP.Common
{
    internal static class Consts
    {
        public static class Log
        {
#if UNITY_EDITOR
            public const string Tag = "<color=#B4FF32>[AI]</color>";
            public static class Color
            {
                public const string TagStart = "<color=#B4FF32>";
                public const string TagEnd = "</color>";

                public const string LevelStart = "<color=#777776>";
                public const string LevelEnd = "</color>";

                public const string CategoryStart = "<color=#007575><b>";
                public const string CategoryEnd = "</b></color>";
            }
#else
            public const string Tag = "[AI]";
            public static class Color
            {
                public const string TagStart = "";
                public const string TagEnd = "";

                public const string LevelStart = "";
                public const string LevelEnd = "";

                public const string CategoryStart = "";
                public const string CategoryEnd = "";
            }
#endif
        }
    }
}