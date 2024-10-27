using UnityEngine;

namespace PersistantManagers
{
	[CreateAssetMenu(menuName = "World/Load Details", fileName = "WorldLoadDetails", order = 0)]
	public class WorldLoadDetails : ScriptableObject
	{
		public GenerationType DetailsType { get; private set; }
		
		public NewWorldDetails NewWorldDetails { get; private set; }
		public LoadWorldDetails LoadWorldDetails { get; private set; }
		
		public void InitializeAsNewWorld(NewWorldDetails details)
		{
			DetailsType = GenerationType.New;
			NewWorldDetails = details;
		}

		public void InitializeAsLoadWorld(LoadWorldDetails details)
		{
			DetailsType = GenerationType.Load;
			LoadWorldDetails = details;
		}
		
		public enum GenerationType { New, Load }
	}

	public readonly struct NewWorldDetails
	{
		public readonly string WorldName;
		public readonly string WorldSeed;

		public NewWorldDetails(string worldName, string worldSeed)
		{
			WorldName = worldName;
			WorldSeed = worldSeed;
		}
	}

	public readonly struct LoadWorldDetails
	{
		public readonly string WorldName;

		public LoadWorldDetails(string worldName)
		{
			WorldName = worldName;
		}
	}
}