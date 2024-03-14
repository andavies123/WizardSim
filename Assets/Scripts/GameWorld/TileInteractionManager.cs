using UnityEngine;

namespace GameWorld
{
	[RequireComponent(typeof(Tile))]
	public class TileInteractionManager : MonoBehaviour
	{
		private static readonly int IsHovered = Shader.PropertyToID("_IsHovered");

		[SerializeField] private Renderer meshRenderer;
		[SerializeField] private MouseInteractionEvents mouseInteractionEvents;

		private Tile _tile;

		private void Awake()
		{
			_tile = GetComponent<Tile>();

			if (mouseInteractionEvents != null)
			{
				mouseInteractionEvents.MouseEntered += OnMouseEntered;
				mouseInteractionEvents.MouseExited += OnMouseExited;
				mouseInteractionEvents.RightMousePressed += OnRightMousePressed;
			}
			else
			{
				Debug.LogError("Unable to subscribe to Mouse Interaction Events. Script is missing...", mouseInteractionEvents);
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
		}

		private void OnMouseEntered() => meshRenderer.material.SetFloat(IsHovered, 1);
		private void OnMouseExited() => meshRenderer.material.SetFloat(IsHovered, 0);
		private void OnRightMousePressed() => print($"Right Mouse Down on {_tile.TilePosition}");
	}
}