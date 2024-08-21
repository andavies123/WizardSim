using System;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HotBarUI
{
	public class HotBarItem : MonoBehaviour
	{
		[SerializeField] private Button button;
		[SerializeField] private GameObject prefab;

		public event EventHandler<HotBarItemSelectedEventArgs> Selected;
		
		private void Awake()
		{
			button.ThrowIfNull(nameof(button));
			prefab.ThrowIfNull(nameof(prefab));
			button.onClick.AddListener(OnButtonClicked);
		}

		private void OnDestroy() => button.onClick.RemoveListener(OnButtonClicked);
		private void OnButtonClicked() => Selected?.Invoke(this, new HotBarItemSelectedEventArgs(prefab));

		public class HotBarItemSelectedEventArgs : EventArgs
		{
			public HotBarItemSelectedEventArgs(GameObject prefab)
			{
				Prefab = prefab;
			}
			
			public GameObject Prefab { get; }
		}
	}
}