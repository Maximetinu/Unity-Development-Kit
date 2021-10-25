using UnityEngine;

namespace UDK
{
	public static class QuaternionExt
	{
		public static Quaternion With(this Quaternion original, float? x = null, float? y = null, float? z = null)
		{
			return Quaternion.Euler(x ?? original.eulerAngles.x, y ?? original.eulerAngles.y, z ?? original.eulerAngles.z);
		}
	}
}