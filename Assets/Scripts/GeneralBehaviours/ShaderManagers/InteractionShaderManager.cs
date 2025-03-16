using System.ComponentModel;
using UI;
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

		public void OverrideBaseColor(Color color)
		{
			baseColor = color;
			meshRenderer.material.SetColor(BaseColor, baseColor);
		}

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
			foreach (Material material in meshRenderer.materials)
			{
				material.SetColor(BaseColor, baseColor);
			}
			
			SetIsHoveredShaderValue(interactable && interactable.IsHovered);

			if (interactable)
				interactable.PropertyChanged += OnInteractablePropertyChanged;
		}

		private void OnDisable()
		{
			if (interactable)
				interactable.PropertyChanged -= OnInteractablePropertyChanged;
		}
		
		private void OnInteractablePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == nameof(Interactable.IsHovered))
				SetIsHoveredShaderValue(interactable.IsHovered);
			if (args.PropertyName == nameof(Interactable.IsSelected))
				SetIsSelectedShaderValue(interactable.IsSelected);
		}

		private void SetIsHoveredShaderValue(bool isHovered)
		{
			foreach (Material material in meshRenderer.materials)
			{
				material.SetFloat(IsHoveredShaderId, ShaderUtilities.BoolToShaderFloat(isHovered));
			}
		}

		private void SetIsSelectedShaderValue(bool isSelected)
		{
			foreach (Material material in meshRenderer.materials)
			{
				material.SetFloat(IsSelectedShaderId, ShaderUtilities.BoolToShaderFloat(isSelected));
			}
		}

		private void SetIsContextMenuOpenShaderValue(bool isContextMenuOpen) =>
			meshRenderer.material.SetFloat(IsContextMenuOpenShaderId, ShaderUtilities.BoolToShaderFloat(isContextMenuOpen));
	}
}