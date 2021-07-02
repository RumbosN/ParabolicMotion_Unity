using System;
using System.Linq;
using Oculus.Platform;
using UnityEngine;

public class PhotonUserPlayerWeapon : UserPlayerWeaponVR
{

	public float cameraRigUpOffset;

    protected Transform _ovrCameraRigTransform;
    protected OvrAvatar _localAvatar;

    protected override void Awake()
    {
        base.Awake();
        _localAvatar = GetComponentInChildren<OvrAvatar>();
    }

    public override void SetupPlayer(EPlayerId playerId, Transform ovrCameraRigTransform)
    {
	    _playerId = playerId;
        _ovrCameraRigTransform = ovrCameraRigTransform;

        var ovrPlayerController = ovrCameraRigTransform.GetComponent<OVRPlayerController>();
        ovrPlayerController.CameraUpdated += UpdateAvatar;

        _leftHandControllerAnchor = ovrCameraRigTransform.FindChildRecursive(Constants.LeftControllerAnchor).GetChild(0);
        _rightHandControllerAnchor = ovrCameraRigTransform.FindChildRecursive(Constants.RightControllerAnchor).GetChild(0);

        SetupCannon();
    }

    public void UpdateAvatar()
    {
        _transform.position = _ovrCameraRigTransform.position + cameraRigUpOffset * Vector3.up;
    }

    protected override void HideHands(bool showHands) {
	    _localAvatar.ShowFirstPerson = showHands;
    }
}
