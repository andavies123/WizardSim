using GameWorld;
using GameWorld.WorldObjects;
using UI.DamageTexts;
using UnityEngine;
using Utilities.Attributes;

namespace Game
{
	[DisallowMultipleComponent]
	public class Managers : MonoBehaviour
	{
		[Header("UI References")]
		[SerializeField, Required] private DamageTextManager damageTextManager;

		[Header("World References")]
		[SerializeField, Required] private World world;
		[SerializeField, Required] private WorldObjectDetailsMap worldObjectDetailsMap;

		// UI References
		public static DamageTextManager DamageTextManager => Instance.damageTextManager;
		
		// World References
		public static World World => Instance.world;
		public static WorldObjectDetailsMap WorldObjectDetailsMap => Instance.worldObjectDetailsMap;
		
		private static Managers Instance { get; set; }

		private void Awake()
		{
			if (Instance) Destroy(this);
			else Instance = this;
		}

		private void OnDestroy() => Instance = null;
	}
}