using Game;
using UnityEngine;

namespace GeneralComponents
{
	public class AlwaysFaceCamera : MonoBehaviour
	{
		private Transform _transform;
		private Camera _camera;

		private void FaceCamera()
		{
			if (!_camera)
				return;
			
			// Get the direction from this object to the camera
			Vector3 directionToCamera = _camera.transform.position - _transform.position;

			// Face the object towards the camera
			_transform.rotation = Quaternion.LookRotation(-directionToCamera);
		}

		private void Awake()
		{
			_transform = transform;
		}

		private void OnEnable()
		{
			if (!_camera)
				_camera = Dependencies.Get<Camera>();
			
			FaceCamera();
		}

		private void Update()
		{
			FaceCamera();
		}
	}
}