using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using WebSocketSharp;
using Random = UnityEngine.Random;

public class Level1Manager : Singleton<Level1Manager> {

	[Header("Setup")]
	[SerializeField] private int _maxNumberTries = 3;
	[SerializeField] private Transform _tableTransform;
	[SerializeField] private Transform _otherGroundTransform;
	[SerializeField] private Transform _cannonTransform;

	[Header("Sample exercises")]
	[SerializeField] private ParabolicVariables[] _samples = Level1Samples.samples;

	[Header("Events")]
	[SerializeField] private EventsResponse _eventsResponse;

	private int[] _tries;
    private int _trie = 0;
    private float toleranceError = 0.001f;

    void Start() {
	    InitTries();
    }

    void Update() {
	    UpdateScreeTexts();
    }

    private void UpdateScreeTexts()
    {}

    private void SetTrie() {
	    var thisTrie = _samples[_tries[_trie]];

	    _otherGroundTransform.position = thisTrie.otherLandPosition;
		_tableTransform.localPosition = new Vector3(_tableTransform.localPosition.x, thisTrie.tableLocalY, _tableTransform.localPosition.z);

		var cannonRotation = _cannonTransform.rotation.eulerAngles;
		cannonRotation.x = thisTrie.alpha;
		_cannonTransform.rotation = Quaternion.Euler(cannonRotation);
    }

    private void InitTries() {
	    _tries = new int[_maxNumberTries];

	    for (int i = 0; i < _maxNumberTries; i++) {
		    _tries[i] = Random.Range(0, _samples.Length);
			print($"Trie {i} -> sample {_tries[i]}");
	    }

	    SetTrie();
    }

    public void ValidateQuestion(TMP_InputField answerField) {
	    if (answerField.text.IsNullOrEmpty()) {
		    _eventsResponse.failedResponseEvent.Invoke();
		}
	    else {
		    var answer = float.Parse(answerField.text);
		    if (Math.Abs(answer - _tries[_trie]) < toleranceError) {
			    _trie += 1;

			    if (_trie == _maxNumberTries) {
				    _eventsResponse.finishedLevelEvent.Invoke();
				}
			    else {
				    _eventsResponse.successfulResponseEvent.Invoke();
				}
		    }
		    else {
			    _eventsResponse.failedResponseEvent.Invoke();
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
