using Extensions;
using UI.ContextMenus;

namespace GameWorld.WorldObjects.Rocks
{
	public class RockContextMenuUser : ContextMenuUser
	{
		private void Awake()
		{
			Rock rock = GetComponent<Rock>();
			
			AddMenuItem(new ContextMenuItem("Destroy", () => rock.gameObject.Destroy()));
		}
	}
}