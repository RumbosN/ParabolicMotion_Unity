using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LevelManager : Singleton<LevelManager>
{

	public bool isDebugMode = true;
	[SerializeField] private GameObject _player;
	[SerializeField] private Transform _playerStartPosition;
	[SerializeField] private UnityEvent _resetLevelEvent;
	[SerializeField] private string _nextLevel;

	[Header("Video")]
	[SerializeField] private bool _thereAreVideo;
	[SerializeField] private GameObject _videoGO;
	[SerializeField] private float _timeToHideVideo;
	[SerializeField] private float _timeToDisappearedVideo = 2.0f;
	[SerializeField] private UnityEvent _beforeVideo;
	[SerializeField] private UnityEvent _afterVideo;


	[Header("Events")]
	public EventsResponse EventsResponse;

	private HashSet<GameObject> _bullets;

	void Awake() {
		_bullets = new HashSet<GameObject>();

		if (_thereAreVideo) {
			_videoGO.SetActive(true);
			_beforeVideo?.Invoke();
			StartCoroutine(DisappearedVideo());
		}
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

	protected IEnumerator DisappearedVideo() {
		yield return new WaitForSeconds(_timeToHideVideo);

		StartCoroutine(HideVideo());
		_afterVideo?.Invoke();

		Material material = _videoGO.GetComponent<MeshRenderer>().material;
		Color color = material.color;
		color.a = 0;
		material.DOColor(color, _timeToDisappearedVideo);
	}

	protected IEnumerator HideVideo() {
		yield return new WaitForSeconds(_timeToDisappearedVideo);
		_videoGO.SetActive(false);
	}
}
