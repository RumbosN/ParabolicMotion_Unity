using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleUserPlayerWeaponVr : UserPlayerWeaponVR {

	[SerializeField] protected GameObject _leftHandMeshRenderer;
	[SerializeField] protected GameObject _rightHandMeshRenderer;

    protected override void Awake() {
		base.Awake();

		SetupCannon();
		SetupPlayer(EPlayerId.PLAYER_1, _transform);
	}

	protected override void HideHands(bool showHands) {
		_leftHandMeshRenderer.SetActive(showHands);
		_rightHandMeshRenderer.SetActive(showHands);
	}

	public override void SetupPlayer(EPlayerId playerId, Transform ovrCameraRigTransform) {
		_leftHandControllerAnchor = _leftHandMeshRenderer.transform;
		_rightHandControllerAnchor = _rightHandMeshRenderer.transform;
	}
}
