using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using WebSocketSharp;
using Random = UnityEngine.Random;

public class Level0Manager : Singleton<Level0Manager> {

	[SerializeField] private int _maxNumberTries = 3;
	[SerializeField] private int _minCliffHeight = 25;
	[SerializeField] private int _maxCliffHeight = 50;
	[SerializeField] private Transform _playerLand;
	[SerializeField] private TextMeshProUGUI _timeText;
	[SerializeField] private TextMeshProUGUI _heightText;
	[SerializeField] private UnityEvent _successfulResponseEvent;
	[SerializeField] private UnityEvent _failedResponseEvent;
	[SerializeField] private UnityEvent _finishedLevelEvent;
	[SerializeField] private int _waitingSecondsToFinishLevel;

	private float[] _tries;
    private int _trie = 0;
    private float toleranceError = 0.001f;

    private float _lastReleaseTime;
    private float _lastImpactTime;

    void Start() {
	    InitTries();
	    _lastReleaseTime = 0.0f;
	    _lastImpactTime = -1.0f;
    }

    void Update() {
	    UpdateScreeTexts();
    }

    private void UpdateScreeTexts()
    {
	    if (_lastReleaseTime > 0 && _lastImpactTime < 0) {
		    _timeText.text = $"{Math.Round(Time.time - _lastReleaseTime, 4, MidpointRounding.AwayFromZero)} sg | {Time.time - _lastReleaseTime}";
	    }
    }

    private void SetTrie() {
        var landPosition = new Vector3(_playerLand.position.x, _tries[_trie] - (_maxCliffHeight / 2.0f), _playerLand.position.z);
	    _playerLand.position = landPosition;
    }

    private void InitTries() {
	    _tries = new float[_maxNumberTries];

	    for (int i = 0; i < _maxNumberTries; i++) {
		    _tries[i] = Random.Range(_minCliffHeight, _maxCliffHeight + 1);
            print($"Trie {i} -> height {_tries[i]}");
	    }

	    SetTrie();
    }

    public void SetLastReleaseTimeAndHeight(float time, float globalHeight) {
	    _lastReleaseTime = time;
	    _lastImpactTime = -1.0f;

	    _heightText.text = $"{Math.Round(globalHeight - _tries[_trie], 2)} m";
	    print($"Release in time {time}");
    }

    public void SetLastImpactTime(float time) {
	    _lastImpactTime = time;
	    print($"Impact in time {time}");
    }

    public void ValidateQuestion(TMP_InputField answerField) {
	    if (answerField.text.IsNullOrEmpty()) {
		    _failedResponseEvent.Invoke();
		}
	    else {
		    var answer = float.Parse(answerField.text);
		    if (Math.Abs(answer - _tries[_trie]) < toleranceError) {
			    _trie += 1;
			    _successfulResponseEvent.Invoke();

			    if (_trie == _maxNumberTries) {
				    StartCoroutine(FinishedLevel());
			    }
			    else {
				    SetTrie();
			    }
		    }
		    else {
			    _failedResponseEvent.Invoke();
		    }
		}
    }

    private IEnumerator FinishedLevel() {
		yield return new WaitForSeconds(_waitingSecondsToFinishLevel);
		_finishedLevelEvent.Invoke();
    }
}
