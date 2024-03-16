using UnityEngine;

public class DisableOnStartup : MonoBehaviour
{
	[SerializeField] private StartupTime startupTime;

	private void Awake()
	{
		if (startupTime == StartupTime.AfterAwake)
			gameObject.SetActive(false);
	}

	private void Start()
	{
		if (startupTime == StartupTime.AfterStart)
			gameObject.SetActive(false);
	}

	public enum StartupTime
	{
		AfterAwake,
		AfterStart
	}
}