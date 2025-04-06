#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#if UNITY_EDITOR
namespace com.IvanMurzak.UnityMCP.Common
{
    public static partial class Consts
    {
        public static partial class Log
        {
            public const string Tag = "<color=#B4FF32>[AI]</color>";
            public static partial class Color
            {
                public const string TagStart = "<color=#B4FF32>";
                public const string TagEnd = "</color>";

                public const string LevelStart = "<color=#777776>";
                public const string LevelEnd = "</color>";

                public const string CategoryStart = "<color=#007575><b>";
                public const string CategoryEnd = "</b></color>";
            }
        }
    }
}
#endif