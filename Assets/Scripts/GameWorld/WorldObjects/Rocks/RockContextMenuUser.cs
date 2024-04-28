using Extensions;
using UI.ContextMenus;

namespace GameWorld.WorldObjects.Rocks
{
	public class RockContextMenuUser : ContextMenuUser
	{
		public override string MenuTitle => "Rock";
		public override string InfoText { get; protected set; } = "---";
		
		private void Awake()
		{
			Rock rock = GetComponent<Rock>();
			
			MenuItems.AddRange(new ContextMenuItem[]
			{
				new("Destroy", () => rock.gameObject.Destroy())
			});
		}
	}
}