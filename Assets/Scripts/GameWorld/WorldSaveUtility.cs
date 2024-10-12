using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameWorld
{
	public static class WorldSaveUtility
	{
		private static string SaveFolderPath => Path.Combine(Application.persistentDataPath, "WorldSaves");
		
		public static void CreateNewSaveFolder(string worldName)
		{
			// Add a random Id as the folder so people can have duplicate world names
			string worldSavePath = Path.Combine(SaveFolderPath, Guid.NewGuid().ToString(), worldName);
			Directory.CreateDirectory(worldSavePath);
		}

		public static void OpenSaveFolder() => EditorUtility.RevealInFinder(SaveFolderPath);
	}
}