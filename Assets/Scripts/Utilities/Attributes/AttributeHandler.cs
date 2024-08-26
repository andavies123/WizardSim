using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Utilities.Attributes
{
	public class AttributeHandler : MonoBehaviour
	{
		[Tooltip("Toggle this to revalidate the attribute search")]
		[SerializeField] private bool revalidate;

		[Header("Attribute Types")]
		[SerializeField] private bool throwIfNull = true;
		
		private void OnValidate()
		{
			if (throwIfNull)
			{
				FindObjectsOfType<MonoBehaviour>()
					.ToList()
					.ForEach(script =>
					{
						script.GetType()
							.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
							.Where(field => Attribute.IsDefined(field, typeof(ThrowIfNullAttribute)))
							.ToList()
							.ForEach(member =>
							{
								if (member.GetValue(script) == null)
								{
									Debug.LogError($"{member.Name} is currently null in the inspector.", script);
								}
							});
					});	
			}
		}
	}
}