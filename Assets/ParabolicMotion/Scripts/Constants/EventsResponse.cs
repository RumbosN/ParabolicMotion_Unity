using UnityEngine.Events;

[System.Serializable]
public struct EventsResponse
{
	public UnityEvent successfulResponseEvent;
	public UnityEvent failedResponseEvent;
	public UnityEvent finishedLevelEvent;
}
