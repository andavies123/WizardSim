using Extensions;
using UI.ContextMenus;

namespace GameWorld.WorldObjects.Rocks
{
	public class RockContextMenuUser : ContextMenuUser
	{
		private void Awake()
		{
			Rock rock = GetComponent<Rock>();
			
			MenuItems.AddRange(new ContextMenuItem[]
			{
				new("Destroy", () => rock.gameObject.Destroy(), AlwaysTrue, AlwaysTrue)
			});
		}
	}
}