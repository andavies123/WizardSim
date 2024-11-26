using UnityEditor;
using UnityEngine;

namespace Editor
{
	public static class EditorExtensions
	{
		/// <summary>
		/// Draws a horizontal line separator
		/// </summary>
		/// <param name="lineHeight">The height of the line. Defaults to 2</param>
		/// <param name="expandWidth">True if the line should expand to fill the width. False if it shouldn't</param>
		public static void DrawHorizontalSeparator(int lineHeight = 2, bool expandWidth = true)
		{
			GUILayout.Box(GUIContent.none, GUILayout.Height(lineHeight), GUILayout.ExpandWidth(expandWidth));
		}

		/// <summary>
		/// Draws a vertical line separator
		/// </summary>
		/// <param name="lineWidth">The width of the line. Defaults to 2</param>
		/// <param name="expandHeight">True if the line should expand to fill height. False if it shouldn't</param>
		public static void DrawVerticalSeparator(int lineWidth = 2, bool expandHeight = true)
		{
			GUILayout.Box(GUIContent.none, GUILayout.Width(lineWidth), GUILayout.ExpandHeight(expandHeight));
		}
		
		/// <summary>
		/// Draws a custom text field where the label is drawn closer to the input field
		/// </summary>
		/// <param name="label">The text that will be displayed on the label</param>
		/// <param name="text">The text that is displayed in the input field</param>
		/// <param name="inputFieldWidth">The width of the input field</param>
		/// <returns>The current input field text</returns>
		public static string TextField(string label, string text, int inputFieldWidth)
		{
			Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
			EditorGUIUtility.labelWidth = textDimensions.x;
			return EditorGUILayout.TextField(label, text, GUILayout.Width(inputFieldWidth));
		}
		
		/// <summary>
		/// Draws a custom dropdown where the label is drawn closer to the dropdown
		/// </summary>
		/// <param name="label">The text that will be displayed on the label</param>
		/// <param name="selectedIndex">The current selected index of the dropdown. (0 to Count - 1)</param>
		/// <param name="displayOptions">Array of drop down options that will be displayed</param>
		/// <param name="dropdownWidth">The width of the dropdown</param>
		/// <returns>The current selected index of the dropdown</returns>
		public static int Dropdown(string label, int selectedIndex, string[] displayOptions, int dropdownWidth)
		{
			Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
			EditorGUIUtility.labelWidth = textDimensions.x;
			return EditorGUILayout.Popup(label, selectedIndex, displayOptions, GUILayout.Width(dropdownWidth));
		}
	}
}