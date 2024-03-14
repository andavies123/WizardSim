using InputStates;
using UnityEngine;

namespace PlayerControllers
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera playerCamera;
		[SerializeField] private GameplayInputState gameplayInputState;
		
		[Header("Speeds")]
		[SerializeField] private float cameraMoveSpeed = 1f;
		[SerializeField] private float cameraRotationSpeed = 1f;
		[SerializeField] private float zoomSpeed = 1f;

		[Header("Clamps")]
		[SerializeField] private float minCameraHeight = 1f;
		[SerializeField] private float maxCameraHeight = 100f;
		[SerializeField] private float minVerticalRotation = 45f;
		[SerializeField] private float maxVerticalRotation = 90f;
		[SerializeField] private float minZoom = 30f;
		[SerializeField] private float maxZoom = 70f;

		private Transform _cameraTransform;
		private float _currentVerticalRotation;
		private float _currentHorizontalRotation;

		private void Awake()
		{
			_cameraTransform = playerCamera.GetComponent<Transform>();

			gameplayInputState.CameraZoomInPerformed += OnCameraZoomInPerformed;
			gameplayInputState.CameraZoomOutPerformed += OnCameraZoomOutPerformed;
		}

		private void Start()
		{
			ClampCameraPosition();
			ClampCameraRotation();
			ClampCameraZoom();
		}

		private void Update()
		{
			if (gameplayInputState.IsMoveActive || gameplayInputState.IsVerticalMoveActive)
				ControlPosition(gameplayInputState.CurrentMoveValue);
			if (gameplayInputState.IsCameraLookActivated && gameplayInputState.IsLookActive)
				ControlRotation(gameplayInputState.CurrentLookValue);
			
			//ControlZoom();
		}

		private void OnDestroy()
		{
			gameplayInputState.CameraZoomInPerformed -= OnCameraZoomInPerformed;
			gameplayInputState.CameraZoomOutPerformed -= OnCameraZoomOutPerformed;
		}

		private void OnCameraZoomInPerformed() => ControlZoom(-1);
		private void OnCameraZoomOutPerformed() => ControlZoom(1);

		private void ControlPosition(Vector3 moveVector)
		{
			Vector3 cameraForward = _cameraTransform.forward;
			
			Vector3 xVector = _cameraTransform.right * moveVector.x;
			Vector3 yVector = Vector3.up * moveVector.y;
			Vector3 zVector = new Vector3(cameraForward.x, 0, cameraForward.z).normalized * moveVector.z;

			// Set position and clamp the height
			Vector3 newPosition = _cameraTransform.position + (xVector + yVector + zVector).normalized * (Time.deltaTime * cameraMoveSpeed);
			_cameraTransform.position = newPosition;
			
			ClampCameraPosition();
		}

		private void ClampCameraPosition()
		{
			Vector3 currentCameraPosition = _cameraTransform.position;
			_cameraTransform.position = new Vector3(currentCameraPosition.x, Mathf.Clamp(currentCameraPosition.y, minCameraHeight, maxCameraHeight), currentCameraPosition.z);
		}

		private void ControlRotation(Vector2 lookVector)
		{
			_currentHorizontalRotation += lookVector.x * cameraRotationSpeed;
			_currentVerticalRotation -= lookVector.y * cameraRotationSpeed;

			_cameraTransform.eulerAngles = new Vector3(_currentVerticalRotation, _currentHorizontalRotation, 0f);
			
			ClampCameraRotation();
		}

		private void ClampCameraRotation()
		{
			Vector3 currentRotation = _cameraTransform.eulerAngles;
			_currentVerticalRotation = Mathf.Clamp(_currentVerticalRotation, minVerticalRotation, maxVerticalRotation);
			_cameraTransform.eulerAngles = new Vector3(_currentVerticalRotation, currentRotation.y, currentRotation.z);
		}

		private void ControlZoom(int zoomAmount)
		{
			playerCamera.fieldOfView += zoomAmount * zoomSpeed;
			
			ClampCameraZoom();
		}

		private void ClampCameraZoom()
		{
			playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView, minZoom, maxZoom);
		}
	}
}