using Unity.VisualScripting;
using UnityEngine;

namespace PlayerControllers
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera playerCamera;
		
		[Header("Speeds")]
		[SerializeField] private float cameraMoveSpeed = 1f;
		[SerializeField] private float cameraRotationSpeed = 1f;

		[Header("Clamps")]
		[SerializeField] private float minVerticalRotation = 45f;
		[SerializeField] private float maxVerticalRotation = 90f;

		private Transform _cameraTransform;

		private void Awake()
		{
			_cameraTransform = playerCamera.GetComponent<Transform>();
		}

		private void Update()
		{
			ControlPosition();
			ControlRotation();
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

			_cameraTransform.position += (xVector + yVector + zVector).normalized * (Time.deltaTime * cameraMoveSpeed);
		}

		private float _currentVerticalRotation;
		private float _currentHorizontalRotation;

		private void ControlRotation()
		{
			if (!Input.GetMouseButton((int) MouseButton.Middle))
				return;

			_currentHorizontalRotation += Input.GetAxisRaw("Mouse X") * cameraRotationSpeed;
			_currentVerticalRotation -= Input.GetAxisRaw("Mouse Y") * cameraRotationSpeed;
			_currentVerticalRotation = Mathf.Clamp(_currentVerticalRotation, minVerticalRotation, maxVerticalRotation);

			_cameraTransform.eulerAngles = new Vector3(_currentVerticalRotation, _currentHorizontalRotation, 0f);
		}
	}
}