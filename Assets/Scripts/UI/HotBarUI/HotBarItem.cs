using System;
using Extensions;
using GameWorld.WorldObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HotBarUI
{
	public class HotBarItem : MonoBehaviour
	{
		[SerializeField] private Button button;
		[SerializeField] private WorldObjectDetails worldObjectDetails;

		public event EventHandler<HotBarItemSelectedEventArgs> Selected;

		public WorldObjectDetails WorldObjectDetails => worldObjectDetails;

		public void Enable() => button.interactable = true;
		public void Disable() => button.interactable = false;
		
		private void Awake()
		{
			button.ThrowIfNull(nameof(button));
			worldObjectDetails.ThrowIfNull(nameof(worldObjectDetails));
			button.onClick.AddListener(OnButtonClicked);
		}

		private void OnDestroy() => button.onClick.RemoveListener(OnButtonClicked);
		private void OnButtonClicked() => Selected?.Invoke(this, new HotBarItemSelectedEventArgs(worldObjectDetails));

		public class HotBarItemSelectedEventArgs : EventArgs
		{
			public HotBarItemSelectedEventArgs(WorldObjectDetails worldObjectDetails)
			{
				WorldObjectWorldObjectDetails = worldObjectDetails;
			}
			
			public WorldObjectDetails WorldObjectWorldObjectDetails { get; }
		}
	}
}