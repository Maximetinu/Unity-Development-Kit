using UnityEngine;

namespace UDK
{
	public static class Vector3Ext
	{
		public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
		{
			return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
		}

		public static Vector3 xz(this Vector3 original)
		{
			return new Vector3(original.x, 0f, original.z);
		}

		public static Vector2 xy(this Vector3 original)
		{
			return new Vector2(original.x, original.y);
		}
	}
}