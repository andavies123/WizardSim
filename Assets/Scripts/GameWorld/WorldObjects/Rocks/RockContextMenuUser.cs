using UI.ContextMenus;
using UnityEngine;

namespace GameWorld.WorldObjects.Rocks
{
	public class RockContextMenuUser : ContextMenuUser<RockContextMenuItem>
	{
		public override string MenuTitle => "Rock";
		public override string InfoText { get; protected set; } = "---";
		
		private void Awake()
		{
			Rock rock = GetComponent<Rock>();
			
			MenuItems.AddRange(new RockContextMenuItem[]
			{
				new ClearRockContextMenuItem(rock, gameObject)
			});
		}
	}

	public abstract class RockContextMenuItem : ContextMenuItem
	{
		protected RockContextMenuItem(Rock rock) => Rock = rock;
		
		protected Rock Rock { get; }
	}

	public class ClearRockContextMenuItem : RockContextMenuItem
	{
		private readonly GameObject _gameObject;

		public ClearRockContextMenuItem(Rock rock, GameObject gameObject) : base(rock) => _gameObject = gameObject;
		
		public override string MenuName => "Clear Rock";

		protected override void OnMenuItemSelected()
		{
			Object.Destroy(_gameObject);
		}
	}
}