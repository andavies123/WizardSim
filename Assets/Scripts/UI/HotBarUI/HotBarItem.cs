using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HotBarUI
{
	public class HotBarItem : MonoBehaviour
	{
		[Header("UI Elements")]
		[SerializeField] private Button button;

		public event EventHandler ButtonClicked;
		
		private void Awake()
		{
			button.onClick.AddListener(OnButtonClicked);
		}

		private void OnDestroy()
		{
			button.onClick.RemoveListener(OnButtonClicked);
		}

		private void OnButtonClicked() => ButtonClicked?.Invoke(this, EventArgs.Empty);
	}
}