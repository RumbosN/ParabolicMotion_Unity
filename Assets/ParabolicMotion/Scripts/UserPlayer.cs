using System;
using System.Linq;
using Oculus.Platform;
using UnityEngine;

public class UserPlayer : MonoBehaviour
{
    public float cameraRigUpOffset;
    
    protected EPlayerId _playerId;
    protected SnowWeapon _snowWeapon;
    protected CannonAza _snowWeaponRightHand;
    protected CannonAza _snowWeaponLeftHand;
    protected Transform _transform;
    protected Transform _ovrCameraRigTransform;
    protected Transform _leftHandControllerAnchor;
    protected Transform _rightHandControllerAnchor;
    protected OvrAvatar _localAvatar;

    private bool _isMovingCannon = false; 

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _localAvatar = GetComponentInChildren<OvrAvatar>();
        _isMovingCannon = false;
    }

    void Update()
    {
        if (_snowWeapon != null)
        {
            if (BattleGameManager.instance.isDebugMode)
            {
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    _snowWeapon.ShotSnowBall();   
                }
            }

            var buttonForce = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) +
                              (Input.GetKey(KeyCode.Space) ? 1.0f : 0.01f);
            _snowWeapon.VelocityAndShoot(buttonForce);
            SnowWeaponMovementTouch();
            SnowWeaponMovementJoystick();
        }
    }

    private void SnowWeaponMovementJoystick()
    {
        Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (OVRInput.Get(OVRInput.Button.Three))
        {
            _snowWeapon.Rotate(secondaryAxis);
        }
    }
    
    private void SnowWeaponMovementTouch()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.9f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.9f)
        {
            if (_snowWeaponRightHand.IsNear(_rightHandControllerAnchor.position) &&
                _snowWeaponLeftHand.IsNear(_leftHandControllerAnchor.position))
            {
                _isMovingCannon = true;
                _snowWeaponLeftHand.ActiveHand();
                _snowWeaponRightHand.ActiveHand();
                _localAvatar.ShowFirstPerson = false;
                _snowWeapon.Rotate(_rightHandControllerAnchor.rotation.eulerAngles) ;
            }
            else
            {
                FinishMovement();
            }
        }
        else
        {
            if (_isMovingCannon)
            {
                FinishMovement();
            }
        }
    }

    private void FinishMovement()
    {
        _snowWeaponLeftHand.InactiveHand();
        _snowWeaponRightHand.InactiveHand();
        _localAvatar.ShowFirstPerson = true;
        _isMovingCannon = false;
    }

    public void SetupPlayer(EPlayerId playerId, Transform ovrCameraRigTransform)
    {
        _playerId = playerId;
        _ovrCameraRigTransform = ovrCameraRigTransform;

        var ovrPlayerController = ovrCameraRigTransform.GetComponent<OVRPlayerController>();
        ovrPlayerController.CameraUpdated += UpdateAvatar;

        _leftHandControllerAnchor = ovrCameraRigTransform.FindChildRecursive(Constants.LeftControllerAnchor).GetChild(0);
        _rightHandControllerAnchor = ovrCameraRigTransform.FindChildRecursive(Constants.RightControllerAnchor).GetChild(0);
        SetupCannon();
    }

    private void SetupCannon()
    {
        _snowWeapon = FindObjectsOfType<SnowWeapon>()
            .First(weapon => weapon.PlayerId == _playerId);

        var cannonAzaChildren = _snowWeapon.GetComponentsInChildren<CannonAza>();
        _snowWeaponLeftHand = cannonAzaChildren.First(aza => aza.HandSide == EHandSide.LEFT);
        _snowWeaponRightHand = cannonAzaChildren.First(aza => aza.HandSide == EHandSide.RIGHT);
    }
    
    public void UpdateAvatar()
    {
        _transform.position = _ovrCameraRigTransform.position + cameraRigUpOffset * Vector3.up;
    }
}
