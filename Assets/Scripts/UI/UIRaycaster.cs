using GeneralBehaviours.HealthBehaviours;
using UI.HealthBars;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.Attributes;

namespace UI
{
	public class UIRaycaster : MonoBehaviour
	{
		[Header("Raycasting components")]
		[SerializeField, Required] private new Camera camera;
		
		[Header("UI Components")]
		[SerializeField, Required] private HealthBarManager healthBarManager;

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
			if (!healthBarManager)
				return;
			
			// Check to see if the Health component exists or if the mouse is over UI
			if (!EventSystem.current.IsPointerOverGameObject() && raycastHit.transform && raycastHit.transform.TryGetComponent(out HealthComponent component))
			{
				healthBarManager.SetHoverHealthBar(component, raycastHit.transform);
			}
			else
			{
				healthBarManager.RemoveHoverHealthBar();
			}
		}
	}
}