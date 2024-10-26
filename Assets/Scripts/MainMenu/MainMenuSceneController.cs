using Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Utilities.Attributes;

namespace MainMenu
{
	public class MainMenuSceneController : MonoBehaviour
	{
		[Header("Prefabs")]
		[SerializeField, Required] private Camera mainCamera;
		[SerializeField, Required] private Light mainLight;
		[SerializeField, Required] private EventSystem eventSystem;

		private void Start()
		{
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.MAIN_MENU_SCENE_NAME));
			
			Instantiate(mainCamera);
			Instantiate(mainLight);
			Instantiate(eventSystem);
		}
	}
}