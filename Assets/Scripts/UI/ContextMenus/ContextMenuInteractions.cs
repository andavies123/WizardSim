using UnityEngine;

namespace UI.ContextMenus
{
	public class ContextMenuInteractions : MonoBehaviour
	{
		private static readonly int IsHovered = Shader.PropertyToID("_IsHovered");
		private static readonly int IsContextMenuOpen = Shader.PropertyToID("_IsContextMenuOpen");
        private static readonly int BaseTexture = Shader.PropertyToID("_BaseTexture");
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
		
		[SerializeField] private MouseInteractionEvents mouseInteractionEvents;
		[SerializeField] private ContextMenuUser contextMenuUser;
		[SerializeField] private Renderer meshRenderer;

		[Header("Shader Properties")]
		[SerializeField] private Texture2D baseTexture;
		[SerializeField] private Color baseColor;

		private void Awake()
		{
			if (mouseInteractionEvents != null)
			{
				mouseInteractionEvents.MouseEntered += OnMouseEntered;
				mouseInteractionEvents.MouseExited += OnMouseExited;
				mouseInteractionEvents.RightMousePressed += OnRightMousePressed;
			}

			if (contextMenuUser != null)
			{
				contextMenuUser.MenuClosed += OnContextMenuClosed;
			}

			if (meshRenderer != null)
			{
				meshRenderer.material.SetTexture(BaseTexture, baseTexture);
				meshRenderer.material.SetColor(BaseColor, baseColor);
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

			if (contextMenuUser != null)
			{
				contextMenuUser.MenuClosed -= OnContextMenuClosed;
			}
		}

		private void OnMouseEntered() => meshRenderer.material.SetFloat(IsHovered, 1);
		private void OnMouseExited() => meshRenderer.material.SetFloat(IsHovered, 0);

		private void OnRightMousePressed()
		{
			if (contextMenuUser == null)
				return;

			contextMenuUser.OpenMenu();
			meshRenderer.material.SetFloat(IsContextMenuOpen, 1);
		}

		private void OnContextMenuClosed()
		{
			meshRenderer.material.SetFloat(IsContextMenuOpen, 0);
		}
	}
}