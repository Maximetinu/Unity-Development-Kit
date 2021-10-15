using UnityEngine;

namespace UDK
{
    public static class ColorExtensions
    {
        public static Color WithOpacityOf(this Color source, Color target)
        {
            return new Color(source.r, source.g, source.b, target.a);
        }

        public static Color WithOpacityOf(this Color source, float opacity)
        {
            return new Color(source.r, source.g, source.b, opacity);
        }

        public static Color AsInverted(this Color source)
        {
            return new Color(1f - source.r, 1f - source.g, 1f - source.b, source.a);
        }

        /// <summary>
        ///  <b>Example:</b> AABBCC
        /// </summary>
        public static string AsHexString(this Color source)
        {
            return ColorUtility.ToHtmlStringRGB(source);
        }

        /// <summary>
        ///  <b>Example:</b> AABBCCFF
        /// </summary>
        public static string AsHexStringAlpha(this Color source)
        {
            return ColorUtility.ToHtmlStringRGBA(source);
        }
    }
}
