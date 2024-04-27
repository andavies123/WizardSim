using UnityEngine;

namespace Extensions
{
	public static class GameObjectExtensions
	{
		public static void Destroy(this GameObject gameObject) => Object.Destroy(gameObject);
	}
}