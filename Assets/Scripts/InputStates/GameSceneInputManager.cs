using System;
using Game;
using UI;
using UnityEngine;

namespace InputStates
{
	public class GameSceneInputManager : MonoBehaviour
	{
		[SerializeField] private GameManager gameManager;
		
		[Header("Events")]
		[SerializeField] private InteractionEvents interactionEvents;

		[Header("Input States")]
		[SerializeField] private GameplayInputState gameplayInputState;
		[SerializeField] private PauseMenuInputState pauseMenuInputState;
		[SerializeField] private InteractionInputState interactionInputState;

		private InputState _activeInputState;
		private Action<MonoBehaviour> _interactionResponse;
		
		private void Awake()
		{
			gameManager.GamePaused += OnGamePaused;
			gameManager.GameResumed += OnGameResumed;
			
			interactionEvents.InteractionRequested += OnInteractionRequested;
			
			interactionInputState.CancelInteractionActionPerformed += OnInteractionCanceled;
			interactionInputState.InteractableSelected += OnInteractableSelected;
			
			SetActiveInputState(gameplayInputState);
		}

		private void OnDestroy()
		{
			gameManager.GamePaused -= OnGamePaused;
			gameManager.GameResumed -= OnGameResumed;

			interactionEvents.InteractionRequested -= OnInteractionRequested;
			
			interactionInputState.CancelInteractionActionPerformed -= OnInteractionCanceled;
			interactionInputState.InteractableSelected -= OnInteractableSelected;
		}
		
		private void OnGamePaused() => SetActiveInputState(pauseMenuInputState);
		private void OnGameResumed() => SetActiveInputState(gameplayInputState);

		private void OnInteractionRequested(Action<MonoBehaviour> interactionResponse)
		{
			_interactionResponse = interactionResponse;
			SetActiveInputState(interactionInputState);
		}
		
		private void OnInteractionCanceled() => SetActiveInputState(gameplayInputState);
		private void OnInteractableSelected(MonoBehaviour monoBehaviour) => _interactionResponse?.Invoke(monoBehaviour);

		private void SetActiveInputState(InputState inputState)
		{
			if (_activeInputState != null)
				_activeInputState.DisableInputs();

			_activeInputState = inputState;
			_activeInputState.EnableInputs();
		}
	}
}