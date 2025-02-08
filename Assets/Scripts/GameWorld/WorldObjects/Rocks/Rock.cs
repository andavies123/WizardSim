using UI.ContextMenus;
using UnityEngine;

namespace GameWorld.WorldObjects.Rocks
{
	[RequireComponent(typeof(WorldObject))]
	[RequireComponent(typeof(ContextMenuUser))]
	public class Rock : MonoBehaviour, IContextMenuUser
	{
		public WorldObject WorldObject { get; private set; }

		private void Awake()
		{
			WorldObject = GetComponent<WorldObject>();
		}
	}
}