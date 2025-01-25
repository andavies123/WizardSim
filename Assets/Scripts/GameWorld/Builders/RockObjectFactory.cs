using Extensions;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld.Builders
{
	public class RockObjectFactory : WorldObjectFactory
	{
		public override string BuilderType => "Rock";

		public override WorldObject CreateObject(Vector2Int chunkPosition, Vector2Int localChunkPosition)
		{
			WorldObjectDetails details = GetRandomDetails();
			WorldObject rock = Instantiate(details.Prefab);
			rock.Init(details, new ChunkPlacementData(chunkPosition, localChunkPosition));
			
			rock.gameObject.name = details.Name;
			
			return rock;
		}
	}
}