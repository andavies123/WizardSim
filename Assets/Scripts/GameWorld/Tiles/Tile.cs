using System.Collections.Generic;
using UI;
using UI.ContextMenus;
using UnityEngine;

namespace GameWorld.Tiles
{
	[RequireComponent(typeof(Interactable))]
	public class Tile : MonoBehaviour, IContextMenuUser
	{
		private Interactable _interactable;
		
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
		}

		private void Awake()
		{
			Transform = transform;
			_interactable = GetComponent<Interactable>();
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