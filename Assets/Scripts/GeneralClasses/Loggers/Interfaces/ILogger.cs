using UnityEngine;

namespace GeneralClasses.Loggers.Interfaces
{
	/// <summary>
	/// Interface to describe a general logger
	/// </summary>
	public interface ILogger
	{
		void Log(string message);
		void Log(string message, object context);
		void Warn(string message);
		void Error(string message);
	}
}