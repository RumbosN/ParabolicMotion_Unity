using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class LevelManager : Singleton<LevelManager>
{

	public bool isDebugMode = true;
	[SerializeField] private GameObject _player;
	[SerializeField] private Transform _playerStartPosition;
	[SerializeField] private UnityEvent _resetLevelEvent;
	[SerializeField] private string _nextLevel;

	[Header("Events")]
	public EventsResponse EventsResponse;

	private HashSet<GameObject> _bullets;

	void Awake() {
		_bullets = new HashSet<GameObject>();
	}

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

	public void AddBullet(GameObject bullet) {
		_bullets.Add(bullet);
	}

	public void RemoveBullet(GameObject bullet) {
		_bullets.Remove(bullet);
	}

	public void ClearBullets() {
		foreach (GameObject bullet in _bullets) {
			Destroy(bullet);
		}

		_bullets.Clear();
	}
}
