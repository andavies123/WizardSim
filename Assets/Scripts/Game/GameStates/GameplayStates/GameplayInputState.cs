using System;
using CameraComponents;
using MessagingSystem;
using UI;
using UI.ContextMenus;
using UI.Messages;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.GameStates.GameplayStates
{
	public class GameplayInputState : IInputState
	{
		private readonly PlayerInputActions _playerInputActions = new();
		private readonly InteractableRaycaster _interactableRaycaster;
		private readonly ISubscription _openContextMenuSubscription;
		private readonly MessageBroker _messageBroker;

		private PlayerInputActions.GameplayActions _gameplay;

		
		public GameplayInputState(InteractableRaycaster interactableRaycaster)
		{
			_interactableRaycaster = interactableRaycaster;
			_gameplay = _playerInputActions.Gameplay;
			_messageBroker = Dependencies.Get<MessageBroker>();

			_openContextMenuSubscription = new SubscriptionBuilder(this)
				.SetMessageType<OpenContextMenuRequest>()
				.SetCallback(OnOpenContextMenuRequestReceived)
				.Build();
		}
		
		public event EventHandler PauseInputPerformed;
		public event EventHandler<Interactable> OpenInfoWindowRequested;
		public event EventHandler<ContextMenuUser> OpenContextMenuRequested;
		public event EventHandler CloseInfoWindowRequested;
		public event EventHandler CloseContextMenuRequested;
		public event EventHandler OpenTaskManagementRequested;
		
		public bool ShowInteractions => true;

		public void Enable()
		{
			_gameplay.PauseGame.performed += OnPauseActionPerformed;
			_gameplay.OpenTaskManagement.performed += OnOpenTaskManagementActionPerformed;
			_gameplay.Cancel.performed += OnCancelActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary += OnInteractablePrimaryActionSelected;
			_interactableRaycaster.InteractableSelectedSecondary += OnInteractableSelectedSecondary;
			_interactableRaycaster.NonInteractableSelectedPrimary += OnNonInteractablePrimaryActionSelected;

			_messageBroker.Subscribe(_openContextMenuSubscription);
			
			_gameplay.Enable();
		}

		public void Disable()
		{
			_gameplay.Disable();
			
			_gameplay.PauseGame.performed -= OnPauseActionPerformed;
			_gameplay.OpenTaskManagement.performed -= OnOpenTaskManagementActionPerformed;
			_gameplay.Cancel.performed -= OnCancelActionPerformed;

			_interactableRaycaster.InteractableSelectedPrimary -= OnInteractablePrimaryActionSelected;
			_interactableRaycaster.InteractableSelectedSecondary -= OnInteractableSelectedSecondary;
			_interactableRaycaster.NonInteractableSelectedPrimary -= OnNonInteractablePrimaryActionSelected;
			
			_messageBroker.Unsubscribe(_openContextMenuSubscription);
		}
		
		private void OnPauseActionPerformed(InputAction.CallbackContext callbackContext)
		{
			PauseInputPerformed?.Invoke(this, EventArgs.Empty);
		}

		private void OnCancelActionPerformed(InputAction.CallbackContext callbackContext)
		{
			CloseInfoWindowRequested?.Invoke(this, EventArgs.Empty);
		}

		private void OnInteractablePrimaryActionSelected(object sender, InteractableRaycasterEventArgs args)
		{
			CloseContextMenuRequested?.Invoke(this, EventArgs.Empty);
			OpenInfoWindowRequested?.Invoke(this, args.Interactable);
		}
		
		private void OnInteractableSelectedSecondary(object sender, InteractableRaycasterEventArgs args)
		{
			if (!args?.Interactable)
				return;
			
			if (args.Interactable.TryGetComponent(out ContextMenuUser contextMenuUser))
				OpenContextMenuRequested?.Invoke(this, contextMenuUser);
		}

		private void OnNonInteractablePrimaryActionSelected(object sender, EventArgs args)
		{
			if (!UIState.IsMouseOverGameObject)
			{
				CloseInfoWindowRequested?.Invoke(this, EventArgs.Empty);
				CloseContextMenuRequested?.Invoke(this, EventArgs.Empty);
			}
		}

		private void OnOpenContextMenuRequestReceived(IMessage message)
		{
			if (message is not OpenContextMenuRequest request)
			{
				Debug.Log($"Message received is not {nameof(OpenContextMenuRequest)}");
				return;
			}
			
			OpenInfoWindowRequested?.Invoke(this, request.ContextMenuUser.Interactable);
		}

		private void OnOpenTaskManagementActionPerformed(InputAction.CallbackContext callbackContext)
		{
			OpenTaskManagementRequested?.Invoke(this, EventArgs.Empty);
		}
	}
}