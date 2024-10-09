using System;

namespace GameObjectPools
{
	public interface IGameObjectPoolItem
	{
		event EventHandler ReleaseRequested;
		void Initialize();
		void CleanUp();
	}
}