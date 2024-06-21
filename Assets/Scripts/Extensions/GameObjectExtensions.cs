using UnityEngine;

namespace Extensions
{
	public static class GameObjectExtensions
	{
		/// <summary>
		/// Extension method shortcut to destroy a game object
		/// </summary>
		/// <param name="gameObject">The extended Game Object</param>
		public static void Destroy(this GameObject gameObject)
		{
			Object.Destroy(gameObject);
		}

		/// <summary>
		/// Extension method to get an existing component on a game object or
		/// add and return a new component
		/// </summary>
		/// <param name="gameObject">The extended Game Object</param>
		/// <typeparam name="T">The type of component to return/add</typeparam>
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			return gameObject.TryGetComponent(out T component) ? component : gameObject.AddComponent<T>();
		}
	}
}