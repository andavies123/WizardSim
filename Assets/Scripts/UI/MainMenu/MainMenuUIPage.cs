using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasScaler))]
	public class MainMenuUIPage : MonoBehaviour
	{
		[Header("Title")]
		[SerializeField] private string pageTitle;
		[SerializeField] private bool hideTitle = false;
		[SerializeField] private TMP_Text titleText;
		
		[Header("Back")]
		[SerializeField] private ButtonPagePair backButtonPagePair;
		
		[Header("Next Pages")]
		[SerializeField] private List<ButtonPagePair> nextButtonPagePairs;
		
		private Canvas _canvas;
		private CanvasScaler _canvasScaler;
		private MainMenuUIManager _uiManager;

		public event Action PageEnabled;
		public event Action PageDisabled;

		public void Enable()
		{
			if (backButtonPagePair.IsComplete)
			{
				backButtonPagePair.Button.onClick.AddListener(() => _uiManager.SetActivePage(backButtonPagePair.NextPage));
			}

			foreach (ButtonPagePair nextButtonPagePair in nextButtonPagePairs)
			{
				if (!nextButtonPagePair.IsComplete)
					continue;
				
				nextButtonPagePair.Button.onClick.AddListener(() => _uiManager.SetActivePage(nextButtonPagePair.NextPage));
			}
			
			PageEnabled?.Invoke();
			_canvas.enabled = true;
		}

		public void Disable()
		{
			_canvas.enabled = false;
			
			if (backButtonPagePair.IsComplete)
			{
				backButtonPagePair.Button.onClick.RemoveAllListeners();
			}

			foreach (ButtonPagePair nextButtonPagePair in nextButtonPagePairs)
			{
				if (!nextButtonPagePair.IsComplete)
					continue;
				
				nextButtonPagePair.Button.onClick.RemoveAllListeners();
			}
			
			PageDisabled?.Invoke();
		}

		protected virtual void Awake()
		{
			_uiManager = GetComponentInParent<MainMenuUIManager>();
			InitializeCanvasObjects();
			InitializeTitle();
			InitializeBackButton();
		}

		protected void OnValidate()
		{
			if (titleText)
			{
				titleText.SetText(pageTitle);
				gameObject.name = pageTitle == string.Empty ? "Main Menu Page" : $"Main Menu - {pageTitle} Page";
			}
			
			InitializeCanvasObjects();
			InitializeTitle();
			InitializeBackButton();
		}

		private void InitializeCanvasObjects()
		{
			_canvas = GetComponent<Canvas>();
			_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			
			_canvasScaler = GetComponent<CanvasScaler>();
			_canvasScaler.referenceResolution = new Vector2(2560, 1440);
			_canvasScaler.matchWidthOrHeight = 0.5f;
		}

		private void InitializeTitle()
		{
			if (!titleText)
				return;
			
			titleText.gameObject.SetActive(!hideTitle);
		}

		private void InitializeBackButton()
		{
			if (backButtonPagePair == null)
				return;

			if (backButtonPagePair.Button)
				backButtonPagePair.Button.gameObject.SetActive(backButtonPagePair.NextPage);
		}
		
		[Serializable]
		private class ButtonPagePair
		{
			[SerializeField] private Button button;
			[SerializeField] private MainMenuUIPage nextPage;

			public Button Button => button;
			public MainMenuUIPage NextPage => nextPage;

			public bool IsComplete => Button && NextPage;
		}
	}
}