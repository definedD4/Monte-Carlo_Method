using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Monte_Carlo_Method_3D.Util.AssertHelper
{
    public static class AssertExtensions
    {
        public static void AssertNotNull(this object @this, string name,
            [CallerFilePath]string filePath = "UNKNOWN", [CallerLineNumber]int lineNumber = -1)
        {
            if (@this == null)
            {
                Debug.Fail($"Assertation failed: {name} can't be null.\n" +
                           $"{filePath}:{lineNumber}");
            }
        }

        public static void AssertNotNullOrWhitespace(this string @this, string name,
            [CallerFilePath]string filePath = "UNKNOWN", [CallerLineNumber]int lineNumber = -1)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                Debug.Fail($"Assertation failed: {name} can't be null or whitespace.\n" +
                           $"{filePath}:{lineNumber}");
            }
        }
    }
}