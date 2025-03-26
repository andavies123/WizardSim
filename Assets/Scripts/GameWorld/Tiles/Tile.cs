using System.Collections.Generic;
using GameWorld.Builders.Chunks;
using GeneralBehaviours.ShaderManagers;
using UI;
using UI.ContextMenus;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Tiles
{
	[RequireComponent(typeof(Interactable))]
	[RequireComponent(typeof(InteractionShaderManager))]
	public class Tile : MonoBehaviour, IContextMenuUser
	{
		[SerializeField, Required] private List<Color> colors;
		
		private Interactable _interactable;
		private InteractionShaderManager _shaderManager;
		
		public World ParentWorld { get; private set; }
		public Chunk ParentChunk { get; private set; }
		public Transform Transform { get; private set; }
		public Vector2Int TilePosition { get; private set; }

		public void Initialize(World parentWorld, Chunk parentChunk, Vector2Int tilePosition)
		{
			ParentWorld = parentWorld;
			ParentChunk = parentChunk;
			TilePosition = tilePosition;

			gameObject.name = $"Tile - {tilePosition}";
			
			InitializeInteractable();
			_shaderManager.OverrideBaseColor(colors[Random.Range(0, colors.Count)]);
		}

		private void Awake()
		{
			Transform = transform;
			_interactable = GetComponent<Interactable>();
			_shaderManager = GetComponent<InteractionShaderManager>();
		}

		private void InitializeInteractable()
		{
			_interactable.TitleText = "Ground";
			_interactable.InfoText = new List<string>
			{
				"Tile",
				$"{TilePosition}"
			};
		}
	}
}