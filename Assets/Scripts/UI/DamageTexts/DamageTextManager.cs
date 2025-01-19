using System.Collections.Generic;
using DamageTypes;
using Extensions;
using UnityEngine;
using Utilities.Attributes;

namespace UI.DamageTexts
{
	public class DamageTextManager : MonoBehaviour
	{
		[SerializeField, Required] private DamageText damageTextPrefab;
		[SerializeField] private float damageTextTimeToLive = 5f;

		private readonly Queue<(DamageText damageText, float timeCreated)> _damageTexts = new();
		private DamageTextFactory _damageTextFactory;

		private void Awake()
		{
			Transform activeContainer = new GameObject("Active Damage Text Container").transform;
			activeContainer.parent = transform;
			
			Transform inactiveContainer = new GameObject("Inactive Damage Text Container").transform;
			inactiveContainer.parent = transform;

			_damageTextFactory = new DamageTextFactory(activeContainer, inactiveContainer, damageTextPrefab.gameObject);
		}

		public void ShowDamageText(Vector3 position, DamageType damageType, float damageAmount)
		{
			Vector3 randomizedPosition = new(position.x + Random.Range(-0.5f, 0.5f), position.y, position.z + Random.Range(-0.5f, 0.5f));
			DamageText damageText = _damageTextFactory.CreateDamageText(randomizedPosition, damageType.DamageTextColor, damageAmount);
			_damageTexts.Enqueue((damageText, Time.time));
		}

		private void Update()
		{
			if (_damageTexts.IsEmpty())
				return;

			while (CanRemoveNextDamageText())
			{
				DamageText damageTextToRelease = _damageTexts.Dequeue().damageText;
				_damageTextFactory.ReleaseDamageText(damageTextToRelease);
			}

			foreach ((DamageText damageText, float timeCreated) in _damageTexts)
			{
				damageText.UpdateWithTime((Time.time - timeCreated)/damageTextTimeToLive);
			}
		}

		private bool CanRemoveNextDamageText()
		{
			return _damageTexts.TryPeek(out (DamageText damageText, float timeCreated) next) && 
			       Time.time - next.timeCreated >= damageTextTimeToLive;
		}
	}
}