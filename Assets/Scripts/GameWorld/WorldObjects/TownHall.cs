using UI.ContextMenus;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	[RequireComponent(typeof(WorldObject))]
	public class TownHall : MonoBehaviour, IContextMenuUser
	{
		public WorldObject WorldObject { get; private set; }

		private void Awake()
		{
			WorldObject = GetComponent<WorldObject>();
		}
	}
}