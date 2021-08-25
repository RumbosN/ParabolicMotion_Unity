using System;
using TMPro;
using UnityEngine;
using WebSocketSharp;
using Random = UnityEngine.Random;

public class Level0Manager : Singleton<Level0Manager> {

	[SerializeField] private int _maxNumberTries = 3;
	[SerializeField] private int _minCliffHeight = 25;
	[SerializeField] private int _maxCliffHeight = 50;
	[SerializeField] private float _topOfCliff = 50.0f;
	[SerializeField] private Transform _playerLand;
	[SerializeField] private float _zeroGroundUp = 0;
	[SerializeField] private Transform _groundTransform;
	[SerializeField] private TextMeshProUGUI _timeText;
	[SerializeField] private TextMeshProUGUI _heightText;

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
		    _timeText.text = $"{Math.Round(Time.time - _lastReleaseTime, 4, MidpointRounding.AwayFromZero)} sg";
	    }
    }

    private void SetTrie() {
	    var groundPosition = new Vector3(_groundTransform.position.x, _maxCliffHeight - _tries[_trie] + _zeroGroundUp, _groundTransform.position.z);
	    _groundTransform.position = groundPosition;
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

	    _heightText.text = $"{((int)((globalHeight - _topOfCliff) * 10)) / 10.0 } m";
	    print($"Release in time {time}");
    }

    public void SetLastImpactTime(float time) {
	    _lastImpactTime = time;
	    print($"Impact in time {time}");
    }

    public void ValidateQuestion(TMP_InputField answerField) {
	    if (answerField.text.IsNullOrEmpty()) {
		    LevelManager.instance?.EventsResponse.failedResponseEvent?.Invoke();
		}
	    else {
		    var answer = float.Parse(answerField.text);
		    if (Math.Abs(answer - _tries[_trie]) < toleranceError) {
			    _trie += 1;

			    if (_trie == _maxNumberTries) {
					LevelManager.instance?.EventsResponse.finishedLevelEvent?.Invoke();
				}
			    else {
					LevelManager.instance?.EventsResponse.successfulResponseEvent?.Invoke();
				}
		    }
		    else {
				LevelManager.instance?.EventsResponse.failedResponseEvent?.Invoke();
		    }
		}
    }

    public void ChangeTrie(int secondsToWait) {
		StartCoroutine(
			LevelManager.instance.WaitToExecute(
				secondsToWait,
				(() => {
					SetTrie();
					LevelManager.instance.ResetLevel(false);
				})
			)
		);
    }
}
