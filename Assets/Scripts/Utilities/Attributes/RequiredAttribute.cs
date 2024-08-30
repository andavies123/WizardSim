using System;
using UnityEngine;

namespace Utilities.Attributes
{
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class RequiredAttribute : PropertyAttribute { }
}