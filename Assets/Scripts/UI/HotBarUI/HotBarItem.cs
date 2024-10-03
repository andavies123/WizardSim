using System;
using GameWorld.WorldObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Attributes;

namespace UI.HotBarUI
{
	public class HotBarItem : MonoBehaviour
	{
		[SerializeField, Required] private Button button;
		[SerializeField, Required] private WorldObjectDetails worldObjectDetails;
		
		public event EventHandler<HotBarItemSelectedEventArgs> Selected;

		public WorldObjectDetails WorldObjectDetails => worldObjectDetails;

		public void Enable() => button.interactable = true;
		public void Disable() => button.interactable = false;
		
		private void Start()
		{
			button.onClick.AddListener(OnButtonClicked);
			InitializeButtonText();
		}

		private void OnValidate()
		{
			if (worldObjectDetails)
			{
				gameObject.name = $"{worldObjectDetails.Name} Hot Bar Item";
				if (button)
				{
					InitializeButtonText();
				}
			}
			else
			{
				gameObject.name = "Hot Bar Item";
			}
		}

		private void OnDestroy() => button.onClick.RemoveListener(OnButtonClicked);
		private void OnButtonClicked() => Selected?.Invoke(this, new HotBarItemSelectedEventArgs(worldObjectDetails));

		private void InitializeButtonText()
		{
			button.GetComponentInChildren<TMP_Text>().SetText(worldObjectDetails.Name);
		}
		
		public class HotBarItemSelectedEventArgs : EventArgs
		{
			public HotBarItemSelectedEventArgs(WorldObjectDetails worldObjectDetails)
			{
				WorldObjectDetails = worldObjectDetails;
			}
			
			public WorldObjectDetails WorldObjectDetails { get; }
		}
	}
}