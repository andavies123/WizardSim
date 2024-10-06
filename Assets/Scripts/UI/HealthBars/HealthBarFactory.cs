using UnityEngine;
using Utilities.Attributes;

namespace UI.HealthBars
{
	public class HealthBarFactory : MonoBehaviour
	{
		[SerializeField, Required] private Transform healthBarParent;
		[SerializeField, Required] private HealthBar healthBarPrefab;

		public HealthBar CreateHealthBar(Vector3 position)
		{
			HealthBar healthBar = Instantiate(healthBarPrefab, position, Quaternion.identity, healthBarParent);
			return healthBar;
		}
	}
}