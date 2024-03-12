using UnityEngine;
using UnityEngine.UIElements;

namespace PlayerControllers
{
	public class CameraController : MonoBehaviour
	{
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

		private Transform _cameraTransform;
		private float _currentVerticalRotation;
		private float _currentHorizontalRotation;

		private void Awake()
		{
			_cameraTransform = playerCamera.GetComponent<Transform>();
		}

		private void Start()
		{
			ClampCameraPosition();
			ClampCameraRotation();
			ClampCameraZoom();
		}

		private void Update()
		{
			ControlPosition();
			ControlRotation();
			ControlZoom();
		}

		private void ControlPosition()
		{
			float xMovement = 0f;
			if (Input.GetKey(KeyCode.A))
				xMovement -= 1;
			if (Input.GetKey(KeyCode.D))
				xMovement += 1;
			
			float zMovement = 0f;
			if (Input.GetKey(KeyCode.S))
				zMovement -= 1;
			if (Input.GetKey(KeyCode.W))
				zMovement += 1;

			float yMovement = 0f;
			if (Input.GetKey(KeyCode.LeftShift))
				yMovement -= 1;
			if (Input.GetKey(KeyCode.Space))
				yMovement += 1;

			Vector3 cameraForward = _cameraTransform.forward;
			
			Vector3 xVector = _cameraTransform.right * xMovement;
			Vector3 yVector = Vector3.up * yMovement;
			Vector3 zVector = new Vector3(cameraForward.x, 0, cameraForward.z).normalized * zMovement;

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

		private void ControlRotation()
		{
			if (!Input.GetMouseButton((int) MouseButton.MiddleMouse))
				return;

			_currentHorizontalRotation += Input.GetAxisRaw("Mouse X") * cameraRotationSpeed;
			_currentVerticalRotation -= Input.GetAxisRaw("Mouse Y") * cameraRotationSpeed;

			_cameraTransform.eulerAngles = new Vector3(_currentVerticalRotation, _currentHorizontalRotation, 0f);
			
			ClampCameraRotation();
		}

		private void ClampCameraRotation()
		{
			Vector3 currentRotation = _cameraTransform.eulerAngles;
			_currentVerticalRotation = Mathf.Clamp(_currentVerticalRotation, minVerticalRotation, maxVerticalRotation);
			_cameraTransform.eulerAngles = new Vector3(_currentVerticalRotation, currentRotation.y, currentRotation.z);
		}

		private void ControlZoom()
		{
			float zoomAmount = Input.GetAxis("Mouse ScrollWheel");
			playerCamera.fieldOfView -= zoomAmount * zoomSpeed;
			
			ClampCameraZoom();
		}

		private void ClampCameraZoom()
		{
			playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView, minZoom, maxZoom);
		}
	}
}