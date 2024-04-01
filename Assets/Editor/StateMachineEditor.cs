using GeneralBehaviours;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[CustomEditor(typeof(StateMachineComponent), true)]
	public class StateMachineEditor : UnityEditor.Editor
	{
		private StateMachineComponent _stateMachineBehaviour;

		public override bool RequiresConstantRepaint() => true;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUILayout.LabelField("State Machine Info", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("Current State:", _stateMachineBehaviour.CurrentStateDisplayName);
			EditorGUILayout.LabelField("State Status:", _stateMachineBehaviour.CurrentStateDisplayStatus);
		}
		
		private void OnEnable()
		{
			_stateMachineBehaviour = (StateMachineComponent) target;
		}
	}
}