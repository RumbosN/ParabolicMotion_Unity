using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : Singleton<LevelManager>
{

	[SerializeField] private GameObject _player;
	[SerializeField] private Transform _playerStartPosition;
	[SerializeField] private UnityEvent _resetLevelEvent;

	public void ResetLevel(bool resetPlayer) {
		_resetLevelEvent.Invoke();

		if (resetPlayer) {
			_player.transform.position = _playerStartPosition.transform.position;
		}
	}

	public IEnumerator WaitToExecute(int timeToWait, Action action) {
		yield return new WaitForSeconds(timeToWait);
		action();
	}
}
