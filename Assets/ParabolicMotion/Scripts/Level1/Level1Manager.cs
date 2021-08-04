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
	[SerializeField] private int _easyLevelUntilTrie = 1;
	[SerializeField] private SnowWeapon _snowWeapon;
	[SerializeField] private int _velocityTriesToEnableInput = 3;


	[Header("Text Variables")]
	[SerializeField] private InputController _velocityInput;
	[SerializeField] private TextMeshProUGUI _distanceText;
	[SerializeField] private TextMeshProUGUI _vxText;
	[SerializeField] private TextMeshProUGUI _voyText;
	[SerializeField] private GameObject _hintEasyLevel;
	[SerializeField] private TextMeshProUGUI _hintEasyLevelText;

	[Header("Sample exercises")]
	[SerializeField] private ParabolicVariables[] _samples = Level1Samples.samples;

	private int[] _tries;
    private int _trie = 0;
    private int _velocityTrie = 0;

    void Start() {
	    InitTries();
	    _velocityInput?.SetDisable(true);
		_snowWeapon?.AddListener2ShootEvent(IncreaseVelocityTrie);
    }

    private void UpdateScreeTexts() {
	    var velocity = _snowWeapon.CurrentVelocity;
	    var alpha = _snowWeapon.GetShootAngle();

	    var decomposedVelocity = PhysicsUtils.DecomposedVelocity(velocity, alpha);
	    _vxText.text = $"{FloatUtils.Floor(decomposedVelocity.x, 2)} m/s";
	    _voyText.text = $"{FloatUtils.Floor(decomposedVelocity.y, 2)} m/s";
    }

    private void SetTrie() {
	    var thisTrie = _samples[_tries[_trie]];

	    _otherGroundTransform.position = thisTrie.otherLandPosition;
		_tableTransform.localPosition = new Vector3(_tableTransform.localPosition.x, thisTrie.tableLocalY, _tableTransform.localPosition.z);

		var cannonRotation = _cannonTransform.rotation.eulerAngles;
		cannonRotation.x = thisTrie.alpha;
		_cannonTransform.rotation = Quaternion.Euler(cannonRotation);

		_distanceText.text = $"{thisTrie.distance} m";

		var isEasyLevel = _trie < _easyLevelUntilTrie;
		_hintEasyLevel.SetActive(isEasyLevel);
		if (isEasyLevel) {
			var vx = PhysicsUtils.DecomposedVelocity(thisTrie.v, thisTrie.alpha).x;
			_hintEasyLevelText.text = $"{FloatUtils.Floor(vx, 2)} m/s";
		}
    }

    private void InitTries() {
	    _tries = new int[_maxNumberTries];

	    for (int i = 0; i < _maxNumberTries; i++) {
		    _tries[i] = Random.Range(0, _samples.Length);
			print($"Trie {i} -> sample {_tries[i]}");
	    }

	    SetTrie();
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

    public void ShootFromScreen() {
	    if (!_velocityInput.Text.IsNullOrEmpty()) {
		    _snowWeapon.SetVelocityAndShoot(float.Parse(_velocityInput.Text));
		    _velocityTrie = 0;
	    }
    }

    public void IncreaseVelocityTrie() {
	    _velocityInput?.SetDisable(++_velocityTrie < _velocityTriesToEnableInput);
	    UpdateScreeTexts();
	}
}
