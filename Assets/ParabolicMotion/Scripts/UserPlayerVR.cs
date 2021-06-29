using System;
using System.Linq;
using Oculus.Platform;
using UnityEngine;

public abstract class UserPlayerVR : MonoBehaviour
{
    protected EPlayerId _playerId;
    protected SnowWeapon _snowWeapon;
    protected CannonAza _snowWeaponRightHand;
    protected CannonAza _snowWeaponLeftHand;
    protected Transform _transform;
    protected Transform _leftHandControllerAnchor;
    protected Transform _rightHandControllerAnchor;

    protected bool _isMovingCannon = false;

    protected virtual void Awake()
    {
        _transform = GetComponent<Transform>();
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

    protected void SnowWeaponMovementJoystick()
    {
        Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (OVRInput.Get(OVRInput.Button.Three))
        {
            _snowWeapon.Rotate(secondaryAxis);
        }
    }

    protected void SnowWeaponMovementTouch()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.9f && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.9f)
        {
            if (_snowWeaponRightHand.IsNear(_rightHandControllerAnchor.position) &&
                _snowWeaponLeftHand.IsNear(_leftHandControllerAnchor.position))
            {
                _isMovingCannon = true;
                _snowWeaponLeftHand.ActiveHand();
                _snowWeaponRightHand.ActiveHand();
                HideHands(false);
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

    protected void FinishMovement()
    {
        _snowWeaponLeftHand.InactiveHand();
        _snowWeaponRightHand.InactiveHand();
        HideHands(true);
        _isMovingCannon = false;
    }

    protected void SetupCannon()
    {
        _snowWeapon = FindObjectsOfType<SnowWeapon>()
            .First(weapon => weapon.PlayerId == _playerId);

        var cannonAzaChildren = _snowWeapon.GetComponentsInChildren<CannonAza>();
        _snowWeaponLeftHand = cannonAzaChildren.First(aza => aza.HandSide == EHandSide.LEFT);
        _snowWeaponRightHand = cannonAzaChildren.First(aza => aza.HandSide == EHandSide.RIGHT);
    }

    public abstract void SetupPlayer(EPlayerId playerId, Transform ovrCameraRigTransform);
    protected abstract void HideHands(bool showHands);
}
