using UI;
using UnityEngine;

namespace GameWorld.Tiles
{
	public partial class Tile : MonoBehaviour
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
		}

		private void Awake()
		{
			Transform = transform;
			_interactable = GetComponent<Interactable>();
		}

		private void Start()
		{
			if (_interactable)
			{
				_interactable.TitleText = "Ground";
				_interactable.InfoText = $"{TilePosition}";
			}
		}
	}
}