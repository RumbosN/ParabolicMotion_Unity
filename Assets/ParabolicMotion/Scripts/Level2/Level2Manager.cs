using TMPro;
using UnityEngine;
using WebSocketSharp;
using Random = UnityEngine.Random;

public class Level2Manager : Singleton<Level2Manager> {

	[Header("Setup")]
	[SerializeField] private int _maxNumberExercises = 3;
	[SerializeField] private Transform _tableTransform;
	[SerializeField] private Transform _otherGroundTransform;
	[SerializeField] private Transform _cannonTransform;
	[SerializeField] private int _easyLevelUntilTrie = 1;
	[SerializeField] private BulletWeapon bulletWeapon;
	[SerializeField] private int _velocityTriesToEnableInput = 3;


	[Header("Text Variables")]
	[SerializeField] private InputController _velocityInput;
	[SerializeField] private TextMeshProUGUI _distanceText;
	[SerializeField] private TextMeshProUGUI _vxText;
	[SerializeField] private TextMeshProUGUI _voyText;
	[SerializeField] private GameObject _hintEasyLevel;
	[SerializeField] private TextMeshProUGUI _hintEasyLevelText;

	[Header("Sample exercises")]
	[SerializeField] private ParabolicVariables[] _samples = ParabolicSamples.samples;

	private int[] _exercises;
    private int _exercise = 0;
    private int _velocityTrie = 0;

    void Start() {
	    InitTries();
	    _velocityInput?.SetDisable(true);
		bulletWeapon?.AddListener2ShootEvent(IncreaseVelocityTrie);
    }

    private void UpdateScreeTexts() {
	    var velocity = bulletWeapon.CurrentVelocity;
	    var alpha = bulletWeapon.GetShootAngle();

	    var decomposedVelocity = PhysicsUtils.DecomposedVelocity(velocity, Mathf.Rad2Deg * alpha);
	    _vxText.text = $"{FloatUtils.Floor(decomposedVelocity.x, 2)} m/s";
	    _voyText.text = $"{FloatUtils.Floor(decomposedVelocity.y, 2)} m/s";
    }

    private void SetExercise() {
		// Rotation X and Y in the cannon is frozen and the user should find the velocity
	    var thisTrie = _samples[_exercises[_exercise]];

	    _otherGroundTransform.position = thisTrie.otherLandPosition;
		_tableTransform.localPosition = new Vector3(_tableTransform.localPosition.x, thisTrie.tableLocalY, _tableTransform.localPosition.z);

		var cannonRotation = _cannonTransform.localRotation.eulerAngles;
		cannonRotation.x = thisTrie.alpha;
		cannonRotation.y = thisTrie.beta;
		_cannonTransform.localRotation = Quaternion.Euler(cannonRotation);

		_distanceText.text = $"{thisTrie.distance} m";

		var isEasyLevel = _exercise < _easyLevelUntilTrie;
		_hintEasyLevel.SetActive(isEasyLevel);
		if (isEasyLevel) {
			var vx = PhysicsUtils.DecomposedVelocity(thisTrie.v, thisTrie.alpha).x;
			_hintEasyLevelText.text = $"{FloatUtils.Floor(vx, 2)} m/s";
		}
    }

    private void InitTries() {
	    _exercises = new int[_maxNumberExercises];

	    for (int i = 0; i < _maxNumberExercises; i++) {
		    _exercises[i] = Random.Range(0, _samples.Length);
			print($"Trie {i} -> sample {_exercises[i]}");
	    }

	    SetExercise();
    }

    public void NextExercise(int secondsToWait) {
	    _exercise++;

	    if (_exercise < _maxNumberExercises) {
		    StartCoroutine(LevelManager.instance.WaitToExecute(secondsToWait, (() => {
			    LevelManager.instance.ResetLevel(false);
			    SetExercise();
		    })));
		}
	    else {
			LevelManager.instance?.EventsResponse.finishedLevelEvent?.Invoke();
	    }
    }

    public void ShootFromScreen() {
	    if (!_velocityInput.Text.IsNullOrEmpty()) {
		    bulletWeapon.SetVelocityAndShoot(float.Parse(_velocityInput.Text));
		    ResetVelocityTrie();
	    }
    }

    public void IncreaseVelocityTrie() {
	    _velocityInput?.SetDisable(++_velocityTrie < _velocityTriesToEnableInput);
	    UpdateScreeTexts();
	}

    public void ResetVelocityTrie() {
	    _velocityTrie = 0;
		_velocityInput?.SetDisable(true);
    }
}
