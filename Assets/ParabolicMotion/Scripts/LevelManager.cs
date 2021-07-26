using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{

	[SerializeField] private GameObject _player;
	[SerializeField] private Transform _playerStartPosition;
	[SerializeField] private UnityEvent _resetLevelEvent;
	[SerializeField] private string _nextLevel;

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

	public void ChangeToNextLevel() {
		SceneFadeManager.instance.FadeOut(2);
		StartCoroutine(WaitToExecute(1, () => SceneManager.LoadScene(_nextLevel)));
	}
}
