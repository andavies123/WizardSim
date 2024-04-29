using Extensions;
using UI;
using UI.ContextMenus;
using UnityEngine;

namespace GameWorld.WorldObjects.Rocks
{
	[RequireComponent(typeof(Interactable))]
	[RequireComponent(typeof(WorldObject))]
	[RequireComponent(typeof(ContextMenuUser))]
	public class Rock : MonoBehaviour
	{
		private WorldObject _worldObject;
		private Interactable _interactable;
		private ContextMenuUser _contextMenuUser;
		
		private void Awake()
		{
			_worldObject = GetComponent<WorldObject>();
			_interactable = GetComponent<Interactable>();
			_contextMenuUser = GetComponent<ContextMenuUser>();
		}

		private void Start()
		{
			InitializeInteractable();
			InitializeContextMenu();
		}

		private void InitializeInteractable()
		{
			_interactable.TitleText = "Rock";
			_interactable.InfoText = "Just a Rock";
		}

		private void InitializeContextMenu()
		{
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Destroy", () => gameObject.Destroy()));
		}
	}
}