using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenuInteractions : MonoBehaviour
	{
		[SerializeField] private MouseInteractionEvents mouseInteractionEvents;
		[SerializeField] private ContextMenuUser contextMenuUser;

		private void Awake()
		{
			if (mouseInteractionEvents != null)
				mouseInteractionEvents.RightMousePressed += OnRightMousePressed;
		}

		private void OnDestroy()
		{
			if (mouseInteractionEvents != null)
				mouseInteractionEvents.RightMousePressed -= OnRightMousePressed;
		}
		
		private void OnRightMousePressed()
		{
			if (contextMenuUser == null)
				return;

			contextMenuUser.OpenMenu();
		}
	}
}