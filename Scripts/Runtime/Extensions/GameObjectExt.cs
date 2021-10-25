using UnityEngine;

namespace UDK
{
	public static class GameObjectExt
	{
		public static void SetActive(this GameObject[] objs, bool active)
		{
			foreach (GameObject obj in objs) obj?.SetActive(active);
		}
	}
}