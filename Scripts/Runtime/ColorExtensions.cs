using System.Collections.Generic;
using System.Linq;
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

        public static byte[] AsByteArray(this IEnumerable<Color> sources)
        {
            int count = sources.Count() * 3;
            int index = 0;
            var array = new byte[count];

            foreach (Color source in sources)
            {
                array[index + 0] = source.r.AsByteColorValue();
                array[index + 1] = source.g.AsByteColorValue();
                array[index + 2] = source.b.AsByteColorValue();
                index += 3;
            }

            return array;
        }

        public static byte[] AsByteArrayAlpha(this IEnumerable<Color> sources)
        {
            int count = sources.Count() * 4;
            int index = 0;
            var array = new byte[count];

            foreach (Color source in sources)
            {
                array[index + 0] = source.r.AsByteColorValue();
                array[index + 1] = source.g.AsByteColorValue();
                array[index + 2] = source.b.AsByteColorValue();
                array[index + 3] = source.a.AsByteColorValue();
                index += 4;
            }

            return array;
        }

        public static byte[] AsByteArray(this Color source)
        {
            return new []
            {
                source.r.AsByteColorValue(),
                source.g.AsByteColorValue(),
                source.b.AsByteColorValue()
            };
        }

        public static byte[] AsByteArrayAlpha(this Color source)
        {
            return new[]
            {
                source.r.AsByteColorValue(),
                source.g.AsByteColorValue(),
                source.b.AsByteColorValue(),
                source.a.AsByteColorValue()
            };
        }

        private static byte AsByteColorValue(this float colorValue)
        {
            return (byte)(colorValue * 255);
        }
    }
}
