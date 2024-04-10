﻿using UI;
using UI.ContextMenus;
using UnityEngine;
using Utilities;

namespace GameWorld.Tiles
{
	public class TileShaderManager : MonoBehaviour
	{
		private static readonly int IsHoveredShaderId = Shader.PropertyToID("_IsHovered");
		private static readonly int IsContextMenuOpenShaderId = Shader.PropertyToID("_IsContextMenuOpen");
		private static readonly int BaseTexture = Shader.PropertyToID("_BaseTexture");
		private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
		
		[Header("Shader Defaults")]
		[SerializeField] private Texture2D baseTexture;
		[SerializeField] private Color baseColor;
		
		[Header("Components")]
		[SerializeField] private MeshRenderer meshRenderer;
		[SerializeField] private Interactable interactable;
		[SerializeField] private ContextMenuInteractions contextMenuInteractions;

		private void Awake()
		{
			if (meshRenderer == null)
			{
				Debug.LogWarning($"{nameof(meshRenderer)} is null. Removing the {nameof(TileShaderManager)} component on this GameObject: {gameObject.name}", this);
				Destroy(this);
			}
		}

		private void OnEnable()
		{
			// Set defaults
			meshRenderer.material.SetTexture(BaseTexture, baseTexture);
			meshRenderer.material.SetColor(BaseColor, baseColor);
			SetIsHoveredShaderValue(interactable && interactable.IsHovered);
			SetIsContextMenuOpenShaderValue(contextMenuInteractions && contextMenuInteractions.IsContextMenuOpen);

			if (interactable)
				interactable.IsHoveredValueChanged += OnIsHoveredValueChanged;

			if (contextMenuInteractions)
				contextMenuInteractions.IsContextMenuOpenValueChanged += OnIsContextMenuOpenValueChanged;
		}

		private void OnDisable()
		{
			if (interactable)
				interactable.IsHoveredValueChanged -= OnIsHoveredValueChanged;
			
			if (contextMenuInteractions)
				contextMenuInteractions.IsContextMenuOpenValueChanged -= OnIsContextMenuOpenValueChanged;
		}

		private void OnIsHoveredValueChanged(bool value) => SetIsHoveredShaderValue(value);
		private void OnIsContextMenuOpenValueChanged(bool value) => SetIsContextMenuOpenShaderValue(value);

		private void SetIsHoveredShaderValue(bool isHovered) =>
			meshRenderer.material.SetFloat(IsHoveredShaderId, ShaderUtilities.BoolToShaderFloat(isHovered));

		private void SetIsContextMenuOpenShaderValue(bool isContextMenuOpen) =>
			meshRenderer.material.SetFloat(IsContextMenuOpenShaderId, ShaderUtilities.BoolToShaderFloat(isContextMenuOpen));
	}
}