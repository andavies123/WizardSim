using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameWorld
{
	public static class WorldSaveUtility
	{
		private static string SaveFolderPath => Path.Combine(Application.persistentDataPath, "WorldSaves");
		private static DirectoryInfo SaveFolderDirectory { get; } = new(SaveFolderPath);
		
		public static void CreateNewSaveFolder(string worldName)
		{
			// Add a random Id as the folder so people can have duplicate world names
			string worldSavePath = Path.Combine(SaveFolderPath, Guid.NewGuid().ToString(), worldName);
			Directory.CreateDirectory(worldSavePath);
		}

		public static void DeleteSaveFolder(string worldId)
		{
			string worldSavePath = Path.Combine(SaveFolderPath, worldId);
			Directory.Delete(worldSavePath, true);
		}

		public static List<WorldSaveQuickDetails> GetAllWorldSaves()
		{
			List<WorldSaveQuickDetails> saveDetails = new();
			DirectoryInfo[] worldSaveDirectories = SaveFolderDirectory.GetDirectories();

			foreach (DirectoryInfo worldSaveDirectory in worldSaveDirectories)
			{
				if (worldSaveDirectory.GetDirectories().Length != 1)
					continue; // We should have only 1 folder in here (the world save)
				
				saveDetails.Add(new WorldSaveQuickDetails
				{
					SaveId = worldSaveDirectory.Name,
					Name = worldSaveDirectory.GetDirectories()[0].Name,
				});
			}

			return saveDetails;
		}
		
		public static void OpenSaveFolder() => EditorUtility.RevealInFinder(SaveFolderPath);
	}

	public class WorldSaveQuickDetails
	{
		public string SaveId { get; set; }
		public string Name { get; set; }
	}
}