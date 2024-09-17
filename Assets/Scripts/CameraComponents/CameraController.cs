using Game;
using Game.GameStates.GameplayStates;
using InputStates.Enums;
using InputStates.InputEventArgs;
using UnityEngine;

namespace CameraComponents
{
	public class CameraController : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private Camera playerCamera;
		
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

		private CameraInputState cameraInputState;
		private Transform _cameraTransform;
		private float _currentVerticalRotation;
		private float _currentHorizontalRotation;

		private void Awake()
		{
			Dependencies.RegisterDependency(playerCamera);
			_cameraTransform = playerCamera.GetComponent<Transform>();
		}

		private void Start()
		{
			cameraInputState = Dependencies.GetDependency<CameraInputState>();
			cameraInputState.CameraZoomInputPerformed += OnCameraZoomInputStatePerformed;
			
			ClampCameraPosition();
			ClampCameraRotation();
			ClampCameraZoom();
		}

		private void Update()
		{
			if (cameraInputState.IsMoveActive || cameraInputState.IsVerticalMoveActive)
				ControlPosition(cameraInputState.CurrentMoveValue);
			if (cameraInputState.IsCameraLookActivated && cameraInputState.IsLookActive)
				ControlRotation(cameraInputState.CurrentLookValue);
		}

		private void OnDestroy()
		{
			cameraInputState.CameraZoomInputPerformed -= OnCameraZoomInputStatePerformed;
		}

		private void OnCameraZoomInputStatePerformed(object sender, CameraZoomInputEventArgs args) => ControlZoom(ZoomTypeToValue(args.Zoom));

		private void ControlPosition(Vector3 moveVector)
		{
			Vector3 cameraForward = _cameraTransform.forward;
			
			Vector3 xVector = _cameraTransform.right * moveVector.x;
			Vector3 yVector = Vector3.up * moveVector.y;
			Vector3 zVector = new Vector3(cameraForward.x, 0, cameraForward.z).normalized * moveVector.z;

			// Set position and clamp the height
			Vector3 newPosition = _cameraTransform.position + (xVector + yVector + zVector).normalized * (Time.unscaledDeltaTime * cameraMoveSpeed);
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
			_currentVerticalRotation = Mathf.Clamp(currentRotation.x, minVerticalRotation, maxVerticalRotation);
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

		private static int ZoomTypeToValue(ZoomType zoomType) => zoomType == ZoomType.In ? -1 : 1;
	}
}