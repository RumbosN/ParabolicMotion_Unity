using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public abstract class BulletWeapon : MonoBehaviourPun
{
	[Header("Weapon Settings")] 
	[SerializeField] protected Transform _middlePoint;
	[SerializeField] protected bool _freeVelocity = false;
    
    [Header("Snow Ball")]
    [SerializeField] protected GameObject _projectilePrefab;
    [SerializeField] protected Transform _projectileSpawn;

    [Header("Rotation")]
    [SerializeField] protected float _maximumHorizontalAngle = 45f;
    [SerializeField] protected float _maximumVerticalAngle = 60f;
    [SerializeField] protected bool _freezeRotationX = false;
    [SerializeField] protected bool _freezeRotationY = false;
    public float rotationAmount = 1.5f;

    [Header("Debug Mode")]
    [SerializeField] protected float _forceVelocityTo = -1.0f;

    protected float _simulationRate = 60f;
    protected float _rotationScaleMultiplier = 1.0f;

    protected bool _isPressed;

    [Header("Velocity")]
    public float maxVelocity = 60.0f;
    public float _velocityIncreaseRate = 5.0f;
    protected float _currentVelocity = 0.0f;
    protected float _timePressed;
    protected int _velocityDirection = 1;

    public float CurrentVelocity => _currentVelocity;

    protected Transform _transform;
    protected UnityEvent ShootEvent;

    protected void Awake() {
	    ShootEvent = new UnityEvent();
    }

    protected void Start()
    {
        _transform = GetComponent<Transform>();
        _isPressed = false;
    }

    public void Rotate(Vector2 axisRotation)
    {
        Vector3 euler = _transform.rotation.eulerAngles;
        float rotateInfluence = _simulationRate * Time.deltaTime * rotationAmount * _rotationScaleMultiplier;

        var newY = euler.y + axisRotation.x * rotateInfluence;
        if (!_freezeRotationY &&
	        (Math.Abs(newY) <= _maximumHorizontalAngle || Math.Abs(newY) >= 360 - _maximumHorizontalAngle))
        {
            euler.y = newY;
        }

        var newX = euler.x - axisRotation.y * rotateInfluence;
        if (!_freezeRotationX &&
            (Math.Abs(newX) <= _maximumHorizontalAngle ||Math.Abs(newX) >= 360 - _maximumHorizontalAngle))
        {
            euler.x = newX;
        }

        _transform.rotation = Quaternion.Euler(euler);
    }

    public void Rotate(Transform leftInteractorHand, Transform rightInteractorHand) {
	    var handsMiddlePoint = (leftInteractorHand.position + rightInteractorHand.position) / 2;
	    var forward = (_middlePoint.position - handsMiddlePoint).normalized;
	    var handsVector = rightInteractorHand.position - leftInteractorHand.position;
	    var normalPlane = Vector3.Cross(forward, Vector3.up);

	    var up = Vector3.Cross(forward, handsVector);
        var up2 = Vector3.ProjectOnPlane(up, normalPlane);

        var newRotation = Quaternion.LookRotation(forward, up2).eulerAngles;
        if (_freezeRotationX) {
	        newRotation.x = _transform.rotation.eulerAngles.x;
        }
        if (_freezeRotationY) {
	        newRotation.y = _transform.rotation.eulerAngles.y;
        }
        _transform.rotation = Quaternion.Euler(newRotation);
    }

    protected Vector3 GetVelocityVector(float shootAngle)
    {
        if (LevelManager.instance.isDebugMode && _forceVelocityTo > 0)
        {
            _currentVelocity = _forceVelocityTo;
        }
        var alpha = shootAngle;
        var fw = _transform.forward;

        var vY = Mathf.Abs(_currentVelocity * Mathf.Sin(alpha));
        var vXUser = _currentVelocity * Mathf.Cos(alpha);

        var tita = Mathf.Atan(fw.x / fw.z);
        var vX = Mathf.Abs(vXUser * Mathf.Sin(tita));
        var vZ = Mathf.Abs(vXUser * Mathf.Cos(tita));

        return new Vector3(vX, vY, vZ);
    }

    public float GetShootAngle()
    {
        var fw = _middlePoint.forward;
        var forwardY0 = new Vector3(fw.x, 0, fw.z).normalized;
        var cosAlpha = Vector3.Dot(fw, forwardY0) / (fw.magnitude * forwardY0.magnitude);
        return Mathf.Acos(Mathf.Clamp(cosAlpha, -1.0f, 1.0f)); // This angle is in radians
    }

    public void VelocityAndShoot(float inputButton)
    {
        if (_isPressed && inputButton < 0.1)
        {
            // Stop to press ==> shoot
            ShotBullet();
        }
        else if (_isPressed && inputButton >= 0.1 && !_freeVelocity)
        {
            // Continue to press ==> increase velocity
            IncreaseVelocity(inputButton);
        }
        else if (!_isPressed && inputButton >= 0.1)
        {
            // Start to press ==> init to increase velocity
            _isPressed = true;
            _timePressed = Time.time;
            _velocityDirection = 1;

            if (!_freeVelocity) {
	            _currentVelocity = 0.0f;
            }
        }
    }

    protected void IncreaseVelocity(float inputButton)
    {
        var deltaTime = Time.time - _timePressed;
        _currentVelocity += _velocityDirection * deltaTime * inputButton * _velocityIncreaseRate;

        if (_currentVelocity >= maxVelocity)
        {
            _currentVelocity = maxVelocity;
            _velocityDirection = -1;
        }

        if (_currentVelocity <= 0.0f)
        {
            _currentVelocity = 0.0f;
            _velocityDirection = 1;
        }

        _timePressed = Time.time;
    }

    public void ShotBullet() {
	    var snowBall = InstantiateSnowBall();
	    IBulletController ballController = snowBall.GetComponent<IBulletController>();

	    var shootAngle = GetShootAngle();
	    ballController?.SetVelocity(GetVelocityVector(shootAngle), shootAngle, _projectileSpawn.forward);
	    _isPressed = false;
	    ShootEvent?.Invoke();

	    LevelManager.instance.AddBullet(snowBall);
    }

    public void SettingAndShoot(float? velocity, float? angle) {
	    if (velocity != null)
	    {
		    _currentVelocity = (float)velocity;
	    }

	    if (angle != null) {
		    var localRotation = _transform.localRotation.eulerAngles;
		    localRotation.x = -(float)angle;
		    _transform.localRotation = Quaternion.Euler(localRotation);
        }

        ShotBullet();
    }

    public void AddListener2ShootEvent(UnityAction action) {
        ShootEvent.AddListener(action);
    }

    public void FreezeVelocity(bool freeze) {
	    _freeVelocity = freeze;
    }

    public void SetVelocity(float velocity) {
	    _currentVelocity = velocity;
    }

    public abstract GameObject InstantiateSnowBall();
}
