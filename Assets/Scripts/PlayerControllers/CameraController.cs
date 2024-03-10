using UnityEngine;

namespace PlayerControllers
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera playerCamera;
		[SerializeField] private float cameraSpeed = 1f;

		private Transform _cameraTransform;

		private void Awake()
		{
			_cameraTransform = playerCamera.GetComponent<Transform>();
		}

		private void Update()
		{
			ControlPosition();
		}

		private void ControlPosition()
		{
			float xMovement = 0f;
			if (Input.GetKey(KeyCode.A))
				xMovement -= Time.deltaTime * cameraSpeed;
			if (Input.GetKey(KeyCode.D))
				xMovement += Time.deltaTime * cameraSpeed;
			
			
			float zMovement = 0f;
			if (Input.GetKey(KeyCode.S))
				zMovement -= Time.deltaTime * cameraSpeed;
			if (Input.GetKey(KeyCode.W))
				zMovement += Time.deltaTime * cameraSpeed;

			float yMovement = 0f;
			if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift))
				yMovement -= Time.deltaTime * cameraSpeed;
			if (Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift))
				yMovement += Time.deltaTime * cameraSpeed;
			
			_cameraTransform.Translate(new Vector3(xMovement, yMovement, zMovement), Space.Self);
		}
	}
}