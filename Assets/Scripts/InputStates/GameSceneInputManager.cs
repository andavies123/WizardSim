using System;
using CameraComponents;
using Game;
using UI;
using UnityEngine;

namespace InputStates
{
	public class GameSceneInputManager : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private InteractableRaycaster interactableRaycaster;
		
		[Header("Events")]
		[SerializeField] private InteractionEvents interactionEvents;

		private IInputState _activeInputState;
		private Action<MonoBehaviour> _interactionResponse;
		
		public GameplayInputState GameplayInputState { get; private set; }
		public PauseMenuInputState PauseMenuInputState { get; private set; }
		public InteractionInputState InteractionInputState { get; private set; }
		
		private void Awake()
		{
			GameplayInputState = new GameplayInputState();
			PauseMenuInputState = new PauseMenuInputState();
			InteractionInputState = new InteractionInputState();
			
			GameManager.Instance.GamePaused += OnGamePaused;
			GameManager.Instance.GameResumed += OnGameResumed;
			
			interactionEvents.InteractionRequested += OnInteractionRequested;
			InteractionInputState.CancelInteractionActionPerformed += OnInteractionCanceled;
			interactableRaycaster.InteractableSelectedPrimary += OnInteractableSelected;
			
			SetActiveInputState(GameplayInputState);
		}

		private void OnDestroy()
		{
			GameManager.Instance.GamePaused -= OnGamePaused;
			GameManager.Instance.GameResumed -= OnGameResumed;

			interactionEvents.InteractionRequested -= OnInteractionRequested;
			InteractionInputState.CancelInteractionActionPerformed -= OnInteractionCanceled;
			interactableRaycaster.InteractableSelectedPrimary -= OnInteractableSelected;
		}
		
		private void OnGamePaused(object sender, EventArgs args) => SetActiveInputState(PauseMenuInputState);
		private void OnGameResumed(object sender, EventArgs args) => SetActiveInputState(GameplayInputState);

		private void OnInteractionRequested(Action<MonoBehaviour> interactionResponse)
		{
			_interactionResponse = interactionResponse;
			SetActiveInputState(InteractionInputState);
		}
		
		private void OnInteractionCanceled() => SetActiveInputState(GameplayInputState);
		private void OnInteractableSelected(MonoBehaviour monoBehaviour) => _interactionResponse?.Invoke(monoBehaviour);

		private void SetActiveInputState(IInputState inputState)
		{
			_activeInputState?.Disable();
			if (_activeInputState?.ShowInteractions ?? false)
				interactableRaycaster.enabled = false;
			
			_activeInputState = inputState;
			
			_activeInputState?.Enable();
			if (_activeInputState?.ShowInteractions ?? false)
				interactableRaycaster.enabled = true;
		}
	}
}