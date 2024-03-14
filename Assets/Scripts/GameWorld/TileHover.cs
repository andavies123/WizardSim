using UnityEngine;

namespace GameWorld
{
	[RequireComponent(typeof(Renderer))]
	public class TileHover : MonoBehaviour
	{
		private static readonly int IsHovered = Shader.PropertyToID("_IsHovered");
		
		private Renderer _renderer;

		private void Awake()
		{
			_renderer = GetComponent<Renderer>();
		}

		private void OnMouseEnter()
		{
			_renderer.material.SetFloat(IsHovered, 1);
		}

		private void OnMouseExit()
		{
			_renderer.material.SetFloat(IsHovered, 0);
		}
	}
}