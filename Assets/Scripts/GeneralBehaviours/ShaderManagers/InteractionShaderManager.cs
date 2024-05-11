using System;
using System.ComponentModel;
using UI;
using UI.ContextMenus;
using UnityEngine;
using Utilities;

namespace GeneralBehaviours.ShaderManagers
{
	public class InteractionShaderManager : MonoBehaviour
	{
		private static readonly int IsHoveredShaderId = Shader.PropertyToID("_IsHovered");
		private static readonly int IsContextMenuOpenShaderId = Shader.PropertyToID("_IsContextMenuOpen");
		private static readonly int IsSelectedShaderId = Shader.PropertyToID("_IsSelected");
		private static readonly int BaseTexture = Shader.PropertyToID("_BaseTexture");
		private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
		
		[Header("Shader Defaults")]
		[SerializeField] private Texture2D baseTexture;
		[SerializeField] private Color baseColor;
		
		[Header("Components")]
		[SerializeField] private MeshRenderer meshRenderer;
		[SerializeField] private Interactable interactable;
		[SerializeField] private ContextMenuUser contextMenuUser;

		private void Awake()
		{
			if (meshRenderer == null)
			{
				Debug.LogWarning($"{nameof(meshRenderer)} is null. Removing the {nameof(InteractionShaderManager)} component on this GameObject: {gameObject.name}", this);
				Destroy(this);
			}
		}

		private void OnEnable()
		{
			// Set defaults
			meshRenderer.material.SetTexture(BaseTexture, baseTexture);
			meshRenderer.material.SetColor(BaseColor, baseColor);
			SetIsHoveredShaderValue(interactable && interactable.IsHovered);
			SetIsContextMenuOpenShaderValue(contextMenuUser && contextMenuUser.IsContextMenuOpen);

			if (interactable)
				interactable.PropertyChanged += OnInteractablePropertyChanged;

			if (contextMenuUser)
				contextMenuUser.IsContextMenuOpenValueChanged += OnIsContextMenuOpenValueChanged;
		}

		private void OnDisable()
		{
			if (interactable)
				interactable.PropertyChanged -= OnInteractablePropertyChanged;
			
			if (contextMenuUser)
				contextMenuUser.IsContextMenuOpenValueChanged -= OnIsContextMenuOpenValueChanged;
		}

		private void OnIsContextMenuOpenValueChanged(object sender, EventArgs args) => 
			SetIsContextMenuOpenShaderValue(contextMenuUser.IsContextMenuOpen);
		
		private void OnInteractablePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == nameof(Interactable.IsHovered))
				SetIsHoveredShaderValue(interactable.IsHovered);
			if (args.PropertyName == nameof(Interactable.IsSelected))
				SetIsSelectedShaderValue(interactable.IsSelected);
		}

		private void SetIsHoveredShaderValue(bool isHovered) =>
			meshRenderer.material.SetFloat(IsHoveredShaderId, ShaderUtilities.BoolToShaderFloat(isHovered));

		private void SetIsSelectedShaderValue(bool isSelected) => 
			meshRenderer.material.SetFloat(IsSelectedShaderId, ShaderUtilities.BoolToShaderFloat(isSelected));

		private void SetIsContextMenuOpenShaderValue(bool isContextMenuOpen) =>
			meshRenderer.material.SetFloat(IsContextMenuOpenShaderId, ShaderUtilities.BoolToShaderFloat(isContextMenuOpen));
	}
}