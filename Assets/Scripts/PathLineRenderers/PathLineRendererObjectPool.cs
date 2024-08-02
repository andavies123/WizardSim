using Extensions;
using GameObjectPools;
using UnityEngine;

namespace PathLineRenderers
{
	public class PathLineRendererObjectPool : MonoBehaviour
	{
		public static PathLineRendererObjectPool Instance { get; private set; }
        
		[SerializeField] private Transform inactiveItemContainer;
		[SerializeField] private Transform activeItemContainer;
		[SerializeField] private PathLineRenderer pathLineRendererPrefab;
        
		private IGameObjectPool pathLineRendererObjectPool;

		public PathLineRenderer GetPathLineRenderer()
		{
			GameObject pathLineRendererGameObject = pathLineRendererObjectPool.GetFromPool(activeItemContainer);
			return pathLineRendererGameObject.GetComponent<PathLineRenderer>();
		}

		public void ReleasePathLineRenderer(PathLineRenderer pathLineRenderer)
		{
			if (!pathLineRenderer)
			{
				Debug.LogWarning($"Unable to release {nameof(PathLineRenderer)} to the object pool... Object is null");
				return;
			}
			
			pathLineRendererObjectPool.ReleaseToPool(pathLineRenderer.gameObject);
		}

		private void Awake()
		{
			if (Instance)
			{
				Debug.LogWarning($"There seems to be multiple instances of {nameof(pathLineRendererObjectPool)}. Removing this instance...", this);
				Destroy(this);
				return;
			}

			Instance = this;
			
			inactiveItemContainer.ThrowIfNull(nameof(inactiveItemContainer));
			pathLineRendererPrefab.ThrowIfNull(nameof(pathLineRendererPrefab));
			
			pathLineRendererObjectPool = new GameObjectPool(
				pathLineRendererPrefab.gameObject,
				inactiveItemContainer,
				5, 10);
		}
	}
}