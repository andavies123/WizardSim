using UI.DamageTexts;
using UnityEngine;
using Utilities.Attributes;

namespace Game
{
	[DisallowMultipleComponent]
	public class Managers : MonoBehaviour
	{
		[SerializeField, Required] private DamageTextManager damageTextManager;

		public static DamageTextManager DamageTextManager => Instance.damageTextManager;
		
		private static Managers Instance { get; set; }

		private void Awake()
		{
			if (Instance) Destroy(this);
			else Instance = this;
		}

		private void OnDestroy() => Instance = null;
	}
}