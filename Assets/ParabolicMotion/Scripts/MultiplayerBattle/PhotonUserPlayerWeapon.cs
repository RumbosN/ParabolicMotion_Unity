using System.Linq;
using UnityEngine;

public class PhotonUserPlayerWeapon : UserPlayerWeaponVR
{

	protected EPlayerId _playerId;

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

    protected override void SetupCannon() {
	    bulletWeapon = FindObjectsOfType<PhotonBulletWeapon>().First(weapon => weapon.PlayerId == _playerId);
	    base.SetupCannon();
    }

    protected override void HideHands(bool showHands) {
	    _localAvatar.ShowFirstPerson = showHands;
    }
}
