using UnityEngine;

namespace UDK
{	
	public static class TransformExt
	{
		public static Vector3 TransformPointUnscaled(this Transform transform, Vector3 position)
		{
			var localToWorldMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
			return localToWorldMatrix.MultiplyPoint3x4(position);
		}

		public static Vector3 InverseTransformPointUnscaled(this Transform transform, Vector3 position)
		{
			var worldToLocalMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse;
			return worldToLocalMatrix.MultiplyPoint3x4(position);
		}

		public static Vector3 TransformPointUnscaledUnrotated(this Transform transform, Vector3 localPosition)
		{
			var localToWorldMatrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
			return localToWorldMatrix.MultiplyPoint3x4(localPosition);
		}

		public static Vector3 InverseTransformPointUnscaledUnrotated(this Transform transform, Vector3 worldPosition)
		{
			var worldToLocalMatrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one).inverse;
			return worldToLocalMatrix.MultiplyPoint3x4(worldPosition);
		}

		public static void SetMatrix(this UnityEngine.Transform transform, UnityEngine.Matrix4x4 matrix)
		{
			transform.localScale = matrix.GetScale();
			transform.rotation = matrix.GetRotation();
			transform.position = matrix.GetPosition();
		}

		public static void SetMatrixLocal(this UnityEngine.Transform transform, UnityEngine.Matrix4x4 matrix)
		{
			transform.localScale = matrix.GetScale();
			transform.localRotation = matrix.GetRotation();
			transform.localPosition = matrix.GetPosition();
		}
	}
}