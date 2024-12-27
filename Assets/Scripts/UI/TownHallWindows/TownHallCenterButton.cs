using GameWorld.Settlements;
using GameWorld.WorldObjects;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.TownHallWindows
{
	[RequireComponent(typeof(Button))]
	public class TownHallCenterButton : MonoBehaviour
	{
		[SerializeField, Required] private Camera mainCamera;
		[SerializeField, Required] private Settlement settlement;
		[SerializeField] private float cameraHeight = 30;
		
		private Button _button;
		private Transform _cameraTransform;
		private TownHall _townHall;

		private void Awake()
		{
			_button = GetComponent<Button>();
			_cameraTransform = mainCamera.transform;
		}

		private void Start()
		{
			_townHall = settlement.TownHall; // Could be null
			settlement.TownHallUpdated += OnTownHallUpdated;
			_button.onClick.AddListener(OnButtonClicked);
			UpdateButtonState();
		}

		private void OnDestroy()
		{
			settlement.TownHallUpdated -= OnTownHallUpdated;
			_button.onClick.RemoveAllListeners();
		}

		private void UpdateButtonState()
		{
			_button.interactable = (bool)_townHall;
		}

		private void OnTownHallUpdated(TownHall townHall)
		{
			_townHall = townHall;
			UpdateButtonState();
		}

		private void OnButtonClicked()
		{
			if (!_townHall)
				return;

			Vector3 townHallCenter = _townHall.WorldObject.PositionDetails.Center;
			Vector3 cameraForward = _cameraTransform.forward;

			// Using SohCahToa to do this calculation (Cah)
			float angleUpRads = Vector3.Angle(cameraForward, Vector3.down) * Mathf.Deg2Rad;
			float height = cameraHeight - townHallCenter.y;
			float distance = height / Mathf.Cos(angleUpRads);

			_cameraTransform.position = townHallCenter - (cameraForward * distance);
		}
	}
}