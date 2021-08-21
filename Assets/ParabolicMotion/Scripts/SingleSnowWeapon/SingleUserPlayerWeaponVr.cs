using UnityEngine;

public class SingleUserPlayerWeaponVr : UserPlayerWeaponVR {

	[SerializeField] protected GameObject _leftHand;
	[SerializeField] protected GameObject _rightHand;
	[SerializeField] protected string handChildModelName;
	[SerializeField] protected string handChildInteractorName;

	protected GameObject _leftRender;
	protected GameObject _rightRender;

	protected override void Awake() {
		base.Awake();

		_leftRender = _leftHand.transform.Find(handChildModelName).gameObject;
		_rightRender = _rightHand.transform.Find(handChildModelName).gameObject;

		SetupCannon();
		SetupPlayer(EPlayerId.PLAYER_1, _transform);
	}

	protected override void HideHands(bool showHands) {
		_leftRender.SetActive(showHands);
		_rightRender.SetActive(showHands);
	}

	public override void SetupPlayer(EPlayerId playerId, Transform ovrCameraRigTransform) {
		_leftHandControllerAnchor = _leftHand.transform.Find(handChildInteractorName);
		_rightHandControllerAnchor = _rightHand.transform.Find(handChildInteractorName);
	}
}
