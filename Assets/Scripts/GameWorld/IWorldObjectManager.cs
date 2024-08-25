using System;
using GameWorld.WorldObjects;

namespace GameWorld
{
	public interface IWorldObjectManager
	{
		/// <summary>
		/// Raised everytime a world object has been added
		/// </summary>
		event EventHandler<WorldObjectManagerEventArgs> WorldObjectAdded;
		
		/// <summary>
		/// Raised everytime a world object has been removed
		/// </summary>
		event EventHandler<WorldObjectManagerEventArgs> WorldObjectRemoved;  
		
		/// <summary>
		/// Adds an world object to the world
		/// </summary>
		/// <param name="worldObject">The world object that should be added</param>
		void AddWorldObject(WorldObject worldObject);
		
		/// <summary>
		/// Removes an existing world object
		/// </summary>
		/// <param name="worldObject">The world object that should be removed</param>
		void RemoveWorldObject(WorldObject worldObject);
		
		/// <summary>
		/// Returns the current amount of existing items of a particular world object
		/// </summary>
		/// <param name="details">The world object details to get the count of</param>
		/// <returns>The amount of existing world objects of this type</returns>
		int GetObjectCount(WorldObjectDetails details);

		/// <summary>
		/// Checks to see if the max amount of this world object have been placed
		/// </summary>
		/// <param name="details">The details of this world object</param>
		/// <returns>True if this world object is already at max capacity. False if not</returns>
		bool IsAtMaxCapacity(WorldObjectDetails details);
	}
	
	public class WorldObjectManagerEventArgs : EventArgs
	{
		public WorldObjectDetails Details { get; set; }
	}
}