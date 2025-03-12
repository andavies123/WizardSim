using GameWorld.WorldObjects;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Builders
{
	public class RockObjectFactory : WorldObjectFactory
	{
		[SerializeField, Required] private WorldObjectPool worldObjectPool;
		
		public override string BuilderType => "Rock";

		public override WorldObject CreateObject(Vector2Int chunkPosition, Vector2Int localChunkPosition)
		{
			WorldObjectDetails details = GetRandomDetails();
			worldObjectPool.TryGetWorldObject(details, out WorldObject rock);
			
			rock.Init(details, new ChunkPlacementData(chunkPosition, localChunkPosition));
			rock.gameObject.SetActive(true);
			
			return rock;
		}
	}
}