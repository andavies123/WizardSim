using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameWorld
{
	public static class WorldSaveUtility
	{
		public const string SaveDetailsFileName = "SaveDetails.json";
		
		private static string SaveFolderPath => Path.Combine(Application.persistentDataPath, "WorldSaves");
		private static DirectoryInfo SaveFolderDirectory { get; } = new(SaveFolderPath);
		
		public static void CreateNewSaveFolder(string worldName)
		{
			WorldSaveDetails details = new()
			{
				saveId = Guid.NewGuid().ToString(),
				name = worldName,
				dateCreated = DateTime.Now.ToString(CultureInfo.CurrentCulture),
				dateLastPlayed = DateTime.Now.ToString(CultureInfo.CurrentCulture)
			};
			
			// Create the initial directory
			string worldSavePath = Path.Combine(SaveFolderPath, details.saveId, details.name);
			Directory.CreateDirectory(worldSavePath);
			
			// Write the save details file
			string detailsPath = Path.Combine(worldSavePath, SaveDetailsFileName);
			string detailsJson = JsonUtility.ToJson(details, true);
			File.WriteAllText(detailsPath, detailsJson);
		}

		public static void DeleteSaveFolder(string worldId)
		{
			string worldSavePath = Path.Combine(SaveFolderPath, worldId);
			Directory.Delete(worldSavePath, true);
		}

		public static List<WorldSaveDetails> GetAllWorldSaves()
		{
			List<WorldSaveDetails> saveDetails = new();
			DirectoryInfo[] worldSaveDirectories = SaveFolderDirectory.GetDirectories();

			foreach (DirectoryInfo worldSaveDirectory in worldSaveDirectories)
			{
				if (worldSaveDirectory.GetDirectories().Length != 1)
					continue; // We should have only 1 folder in here (the world save)

				string saveDetailsPath =
					Path.Combine(worldSaveDirectory.GetDirectories()[0].FullName, SaveDetailsFileName);
				
				string detailsJson = File.ReadAllText(saveDetailsPath);
				WorldSaveDetails worldSaveDetails = JsonUtility.FromJson<WorldSaveDetails>(detailsJson);
				saveDetails.Add(worldSaveDetails);
			}

			return saveDetails;
		}
		
		public static void OpenSaveFolder() => EditorUtility.RevealInFinder(SaveFolderPath);
	}

	[Serializable]
	public class WorldSaveDetails
	{
		public string saveId;
		public string name;
		public string dateCreated;
		public string dateLastPlayed;
	}
}