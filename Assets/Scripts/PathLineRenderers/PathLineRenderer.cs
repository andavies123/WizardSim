using UnityEngine;

namespace PathLineRenderers
{
	[RequireComponent(typeof(LineRenderer))]
	public class PathLineRenderer : MonoBehaviour
	{
		[SerializeField] private float animationSpeed = 1f;
        
		private LineRenderer _lineRenderer;

		public void UpdateLine(Vector3[] linePoints)
		{
			if (linePoints == null || linePoints.Length == 0)
				return;
			
			_lineRenderer.SetPositions(linePoints);
		}

		private void Awake()
		{
			_lineRenderer = GetComponent<LineRenderer>();
		}

		private void Update()
		{
			_lineRenderer.material.mainTextureOffset = Time.time * animationSpeed * Vector2.right;
		}
	}
}