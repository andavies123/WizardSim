using UI.ContextMenus;
using UnityEngine;

namespace GameWorld.WorldObjects
{
	public class RockContextMenuUser : ContextMenuUser<RockContextMenuItem>
	{
		public override string MenuTitle => "Rock";
		public override string InfoText { get; protected set; } = "---";
		
		private void Awake()
		{
			MenuItems.AddRange(new RockContextMenuItem[]
			{
				new ClearRockContextMenuItem(gameObject)
			});
		}
	}

	public abstract class RockContextMenuItem : ContextMenuItem
	{
		
	}

	public class ClearRockContextMenuItem : RockContextMenuItem
	{
		private readonly GameObject _gameObject;

		public ClearRockContextMenuItem(GameObject gameObject) => _gameObject = gameObject;
		
		public override string MenuName => "Clear Rock";

		protected override void OnMenuItemSelected()
		{
			Object.Destroy(_gameObject);
		}
	}
}