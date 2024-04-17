using HealthComponents;
using UI.HealthBars;
using UnityEngine;

namespace UI
{
	public class UIRaycaster : MonoBehaviour
	{
		[Header("Raycasting components")]
		[SerializeField] private new Camera camera;
		
		[Header("UI Components")]
		[SerializeField] private WorldHealthBar worldHealthBar;

		private void Awake()
		{
			if (!camera)
			{
				Debug.LogWarning($"{nameof(UIRaycaster)}: {nameof(camera)} does not exist. Setting component to inactive");
				enabled = false;
			}
            
			if (!worldHealthBar)
				Debug.LogWarning($"{nameof(UIRaycaster)}: {nameof(worldHealthBar)} does not exist");
		}

		private void Update()
		{
			// Get a ray from the mouse through the screen
			Ray screenRay = camera.ScreenPointToRay(Input.mousePosition);
			
			// Send out a raycast
			Physics.Raycast(screenRay, out RaycastHit raycastHit, 100);
			
			// Update the necessary UIs
			UpdateHealthBar(raycastHit);
		}

		private void UpdateHealthBar(RaycastHit raycastHit)
		{
			// Check to make sure the necessary HealthBar Script exists
			if (!worldHealthBar)
				return;

			Health health = null;

			// Check to see if the Health component exists
			if (raycastHit.transform)
				raycastHit.transform.TryGetComponent(out health);
			
			// Update the health bar
			worldHealthBar.SetHealth(health);
		}
	}
}