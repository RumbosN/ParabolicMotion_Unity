using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : Singleton<LevelManager>
{

	[SerializeField] private GameObject _player;
	[SerializeField] private Transform _playerStartPosition;
	[SerializeField] private UnityEvent _resetLevelEvent;

	public void ResetLevel() {
		_resetLevelEvent.Invoke();
		Task.Delay(3000).Wait();
		_player.transform.position = _playerStartPosition.position;
	}

	public IEnumerator WaitToExecute(int timeToWait, Action action) {
		yield return new WaitForSeconds(timeToWait);
		action();
	}
}
