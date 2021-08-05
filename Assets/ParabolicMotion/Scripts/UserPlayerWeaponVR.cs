using System;
using System.Linq;
using Oculus.Platform;
using UnityEngine;

public abstract class UserPlayerWeaponVR : MonoBehaviour
{
    protected BulletWeapon bulletWeapon;
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
        if (bulletWeapon != null)
        {
            if (LevelManager.instance.isDebugMode)
            {
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    bulletWeapon.ShotBullet();
                }
            }

            var buttonForce = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) +
                              (Input.GetKey(KeyCode.Space) ? 1.0f : 0.01f);
            bulletWeapon.VelocityAndShoot(buttonForce);
            SnowWeaponMovementTouch();
            SnowWeaponMovementJoystick();
        }
    }

    protected void SnowWeaponMovementJoystick()
    {
        Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (OVRInput.Get(OVRInput.Button.Three))
        {
            bulletWeapon.Rotate(secondaryAxis);
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
                bulletWeapon.Rotate(_leftHandControllerAnchor, _rightHandControllerAnchor) ;
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

    protected virtual void SetupCannon()
    {
        bulletWeapon = FindObjectsOfType<BulletWeapon>().First();

        var cannonAzaChildren = bulletWeapon.GetComponentsInChildren<CannonAza>();
        _snowWeaponLeftHand = cannonAzaChildren.First(aza => aza.HandSide == EHandSide.LEFT);
        _snowWeaponRightHand = cannonAzaChildren.First(aza => aza.HandSide == EHandSide.RIGHT);
    }

    public abstract void SetupPlayer(EPlayerId playerId, Transform ovrCameraRigTransform);
    protected abstract void HideHands(bool showHands);
}
