using UI;
using UnityEngine;

namespace GameWorld
{
	[RequireComponent(typeof(Tile))]
	public class TileInteractionManager : MonoBehaviour
	{
		private static readonly int IsHovered = Shader.PropertyToID("_IsHovered");
		private static readonly int IsContextMenuOpen = Shader.PropertyToID("_IsContextMenuOpen");

		[SerializeField] private Renderer meshRenderer;
		[SerializeField] private MouseInteractionEvents mouseInteractionEvents;
		[SerializeField] private TileContextMenuUser tileContextMenuUser;

		private void Awake()
		{
			if (mouseInteractionEvents != null)
			{
				mouseInteractionEvents.MouseEntered += OnMouseEntered;
				mouseInteractionEvents.MouseExited += OnMouseExited;
				mouseInteractionEvents.RightMousePressed += OnRightMousePressed;
			}

			if (tileContextMenuUser != null)
			{
				tileContextMenuUser.MenuClosed += OnContextMenuClosed;
			}
		}

		private void OnDestroy()
		{
			if (mouseInteractionEvents != null)
			{
				mouseInteractionEvents.MouseEntered -= OnMouseEntered;
				mouseInteractionEvents.MouseExited -= OnMouseExited;
				mouseInteractionEvents.RightMousePressed -= OnRightMousePressed;
			}

			if (tileContextMenuUser != null)
			{
				tileContextMenuUser.MenuClosed -= OnContextMenuClosed;
			}
		}

		private void OnMouseEntered() => meshRenderer.material.SetFloat(IsHovered, 1);
		private void OnMouseExited() => meshRenderer.material.SetFloat(IsHovered, 0);

		private void OnRightMousePressed()
		{
			if (tileContextMenuUser == null)
				return;

			tileContextMenuUser.OpenMenu();
			meshRenderer.material.SetFloat(IsContextMenuOpen, 1);
		}

		private void OnContextMenuClosed()
		{
			meshRenderer.material.SetFloat(IsContextMenuOpen, 0);
		}
	}
}