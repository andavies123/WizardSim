using UI;
using UnityEngine;

namespace GameWorld.WorldObjects.Rocks
{
	[RequireComponent(typeof(Interactable))]
	[RequireComponent(typeof(WorldObject))]
	public class Rock : MonoBehaviour
	{
		private WorldObject _worldObject;
		private Interactable _interactable;

		private void Awake()
		{
			_worldObject = GetComponent<WorldObject>();
			_interactable = GetComponent<Interactable>();
		}

		private void Start()
		{
			InitializeInteractable();
		}

		private void InitializeInteractable()
		{
			_interactable.TitleText = "Rock";
			_interactable.InfoText = "Just a Rock";
		}
	}
}