using System;
using CameraComponents;
using Extensions;
using UI.ContextMenus;
using UnityEngine;

namespace Game.GameStates.ContextMenuStates
{
	public class ContextMenuGameState : GameState
	{
		private readonly ContextMenuUIState _contextMenuUIState;
		private readonly ContextMenuInputState _contextMenuInputState;

		public event EventHandler MenuClosed;
        
		public ContextMenuGameState(ContextMenuUIState uiState, InteractableRaycaster interactableRaycaster)
		{
			_contextMenuUIState = uiState.ThrowIfNull(nameof(uiState));
			_contextMenuInputState = new ContextMenuInputState(interactableRaycaster);
		}

		public override bool AllowCameraInputs => false;
		public override bool AllowInteractions => true;

		protected override UIState UIState => _contextMenuUIState;
		protected override IInputState InputState => _contextMenuInputState;

		public void Initialize(ContextMenuUser contextMenuUser, Vector2 screenPosition)
		{
			OnOpenNewContextMenuRequested(this, contextMenuUser);
			//_contextMenuUIState.ContextMenu.Initialize(contextMenuUser, screenPosition);
		}

		protected override void OnEnabled()
		{
			_contextMenuInputState.NavigationActionPerformed += OnNavigationInputPerformed;
			_contextMenuInputState.CloseRequested += OnCloseInputPerformed;
			_contextMenuInputState.SelectActionPerformed += OnSelectInputPerformed;
			_contextMenuInputState.OpenNewContextMenuRequested += OnOpenNewContextMenuRequested;

			_contextMenuUIState.ContextMenuClosed += OnContextMenuClosed;
		}

		protected override void OnDisabled()
		{
			_contextMenuInputState.NavigationActionPerformed -= OnNavigationInputPerformed;
			_contextMenuInputState.CloseRequested -= OnCloseInputPerformed;
			_contextMenuInputState.SelectActionPerformed -= OnSelectInputPerformed;
			_contextMenuInputState.OpenNewContextMenuRequested -= OnOpenNewContextMenuRequested;

			_contextMenuUIState.ContextMenuClosed -= OnContextMenuClosed;
		}
		
		private void OnNavigationInputPerformed(object sender, ContextMenuNavigationEventArgs args)
		{
			switch (args.NavigationOption)
			{
				case NavigationOption.Up:
					_contextMenuUIState.ContextMenu.NavigateUp();
					break;
				case NavigationOption.Down: 
					_contextMenuUIState.ContextMenu.NavigateDown();
					break;
				case NavigationOption.Left:
					_contextMenuUIState.ContextMenu.NavigateBack();
					break;
				case NavigationOption.Right:
					_contextMenuUIState.ContextMenu.NavigateForward();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(args.NavigationOption), args.NavigationOption, "Enum case does not exist");
			}
		}

		private void OnSelectInputPerformed(object sender, EventArgs args)
		{
			_contextMenuUIState.ContextMenu.SelectCurrentItem();
		}

		private void OnCloseInputPerformed(object sender, EventArgs args)
		{
			_contextMenuUIState.ContextMenu.CloseMenu();
		}

		// Todo: Update to not have ContextMenuUser
		private void OnOpenNewContextMenuRequested(object sender, ContextMenuUser contextMenuUser)
		{
			if (contextMenuUser.TryGetComponent(out IContextMenuUser contextMenuUserInt))
			{
				if (Globals.ContextMenuInjections.TryGetMenuItemTreeByType(contextMenuUserInt.GetType().Name, out ContextMenuItemTree menuItemTree))
					_contextMenuUIState.ContextMenu.Initialize(contextMenuUserInt, menuItemTree, Input.mousePosition);
				else
					Debug.Log($"Type not found: {contextMenuUserInt.GetType().Name}");
			}
		}

		private void OnContextMenuClosed(object sender, EventArgs args)
		{
			MenuClosed?.Invoke(sender, args);
		}
	}
}