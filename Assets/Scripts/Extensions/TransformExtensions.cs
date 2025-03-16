using UnityEngine;

namespace Extensions
{
	public static class TransformExtensions
	{
		public static bool TryGetComponentInParent<T>(this Transform transform, out T component) where T : Component
		{
			if (!transform)
			{
				component = null;
				return false;
			}

			component = transform.GetComponentInParent<T>();
			return component;
		}
	}
}