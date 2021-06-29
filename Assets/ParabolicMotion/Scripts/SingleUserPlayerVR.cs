using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleUserPlayerVR : UserPlayerVR {

	[SerializeField] protected MeshRenderer _leftHandMeshRenderer;
	[SerializeField] protected MeshRenderer _rightHandMeshRenderer;

    protected override void Awake() {
		base.Awake();

		SetupCannon();
		SetupPlayer(EPlayerId.PLAYER_1, _transform);
	}

	protected override void HideHands(bool showHands) {
		_leftHandMeshRenderer.enabled = showHands;
		_rightHandMeshRenderer.enabled = showHands;
	}

	public override void SetupPlayer(EPlayerId playerId, Transform ovrCameraRigTransform) {
		_leftHandControllerAnchor = _leftHandMeshRenderer.transform;
		_rightHandControllerAnchor = _rightHandMeshRenderer.transform;
	}
}
