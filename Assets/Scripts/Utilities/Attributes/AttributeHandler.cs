using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities.Attributes
{
	public class AttributeHandler : MonoBehaviour
	{
		[Tooltip("Toggle this to revalidate the attribute search")]
		[SerializeField] private bool revalidate;

		[Header("Attribute Types")]
		[SerializeField] private bool required = true;
		
		private void OnValidate()
		{
			FindObjectsOfType<MonoBehaviour>().ToList().ForEach(CheckScript);
		}

		private void CheckScript(MonoBehaviour script)
		{
			Type scriptType = script.GetType();

			FieldInfo[] fields = scriptType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			
			fields.Where(field => Attribute.IsDefined(field, typeof(RequiredAttribute)))
				.ToList()
				.ForEach(field => HandleThrowIfNull(field, script));
		}

		private void HandleThrowIfNull(FieldInfo fieldInfo, Object script)
		{
			if (!required)
				return;

			if (fieldInfo.GetValue(script) == null)
			{
				Debug.LogError($"\"{fieldInfo.Name}\" is not set in the inspector for the component \"{script.GetType().Name}\"", script);
			}
		}
	}
}