using GameWorld.Settlements;
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
		
		private Button _button;
		private Transform _cameraTransform;

		private void Awake()
		{
			_button = GetComponent<Button>();
			_cameraTransform = mainCamera.transform;
		}

		private void OnEnable()
		{
			_button.onClick.AddListener(OnButtonClicked);
		}

		private void OnDisable()
		{
			_button.onClick.RemoveAllListeners();
		}

		private void OnButtonClicked()
		{
			if (!settlement.TownHall)
				return;

			Vector3 townHallCenter = settlement.TownHall.WorldObject.PositionDetails.Center;
			Vector3 cameraForward = _cameraTransform.forward;

			// Using SohCahToa to do this calculation (Cah)
			float angleUpRads = Vector3.Angle(cameraForward, Vector3.down) * Mathf.Deg2Rad;
			float height = _cameraTransform.position.y - townHallCenter.y;
			float distance = height / Mathf.Cos(angleUpRads);

			_cameraTransform.position = townHallCenter - (cameraForward * distance);
		}
	}
}