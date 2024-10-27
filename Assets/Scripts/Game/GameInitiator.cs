using Cysharp.Threading.Tasks;
using Game.EventChannels;
using PersistantManagers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Attributes;

namespace Game
{
	public class GameInitiator : MonoBehaviour
	{
		[SerializeField, Required]
		private StringEventChannel sceneChangeRequestChannel;

		private async void Start()
		{
			if (!SceneManager.GetSceneByName(SceneNames.PERSISTANT_MANAGERS_SCENE_NAME).isLoaded)
			{
				await SceneManager.LoadSceneAsync(SceneNames.PERSISTANT_MANAGERS_SCENE_NAME, LoadSceneMode.Additive);
			}
			
			sceneChangeRequestChannel.Raise(this, SceneNames.MAIN_MENU_SCENE_NAME);
			await SceneManager.UnloadSceneAsync(SceneNames.CONTROL_SCENE_NAME);
		}
	}
}