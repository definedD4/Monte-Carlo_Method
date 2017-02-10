using System;

namespace Monte_Carlo_Method_3D.Util.AssertHelper
{
    public static class AssertExtensions
    {
        public static void AssertNotNull(this object @this, string name)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(name, $"{name} can't be null.");
            }
        }

        public static void AssertNotNullOrWhitespace(this string @this, string name)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                throw new ArgumentException($"{name} can't be null or whitespace.", name);
            }
        }
    }
}