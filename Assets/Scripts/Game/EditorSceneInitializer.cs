using System.Diagnostics;
using PersistantManagers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	public class EditorSceneInitializer : MonoBehaviour
	{
		private void Awake()
		{
			LoadPersistantManagers();
		}

		[Conditional("UNITY_EDITOR")]
		private static void LoadPersistantManagers()
		{
			Scene scene = SceneManager.GetSceneByName(SceneNames.PERSISTANT_MANAGERS_SCENE_NAME);

			if (scene.isLoaded)
				return;

			SceneManager.LoadScene(SceneNames.PERSISTANT_MANAGERS_SCENE_NAME, LoadSceneMode.Additive);
		}
	}
}