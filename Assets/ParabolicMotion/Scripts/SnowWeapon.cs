using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public abstract class SnowWeapon : MonoBehaviourPun
{
    [SerializeField] protected EPlayerId _playerId;

    [Header("Snow Ball")]
    [SerializeField] protected GameObject _snowBallPrefab;
    [SerializeField] protected Transform _snowBallSpawn;

    [Header("Rotation")]
    [SerializeField] protected float _maximumHorizontalAngle = 45f;
    [SerializeField] protected float _maximumVerticalAngle = 60f;
    [SerializeField] protected bool _freezeRotationX = false;
    [SerializeField] protected bool _freezeRotationY = false;
    public float rotationAmount = 1.5f;

    protected float _simulationRate = 60f;
    protected float _rotationScaleMultiplier = 1.0f;

    protected bool _isPressed;

    [Header("Velocity")]
    public float maxVelocity = 60.0f;
    public float _velocityIncreaseRate = 5.0f;
    protected float _currentVelocity = 0.0f;
    protected float _timePressed;
    protected int _velocityDirection = 1;

    public EPlayerId PlayerId => _playerId;
    public float CurrentVelocity => _currentVelocity;

    protected Transform _transform;

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

    public void Rotate(Vector3 newRotationEuler)
    {
        Vector3 euler = _transform.rotation.eulerAngles;

        if (!_freezeRotationX) {
	        euler.x = newRotationEuler.x;
        }

        if (!_freezeRotationX) {
	        euler.y = newRotationEuler.y;
        }

        _transform.rotation = Quaternion.Euler(euler);
    }

    protected Vector3 GetVelocityVector(float shootAngle)
    {
        var alpha = shootAngle;
        var fw = _transform.forward;

        _currentVelocity = 25.0f;
        var vY = Mathf.Abs(_currentVelocity * Mathf.Sin(alpha));
        var vXUser = _currentVelocity * Mathf.Cos(alpha);

        var tita = Mathf.Atan(fw.x / fw.z);
        var vX = Mathf.Abs(vXUser * Mathf.Sin(tita));
        var vZ = Mathf.Abs(vXUser * Mathf.Cos(tita));

        return new Vector3(vX, vY, vZ);
    }

    public float GetShootAngle()
    {
        var fw = _transform.forward;
        var forwardY0 = new Vector3(fw.x, 0, fw.z).normalized;
        var cosAlpha = Vector3.Dot(fw, forwardY0) / (fw.magnitude * forwardY0.magnitude);
        return Mathf.Acos(cosAlpha); // This angle is in radians
    }

    public void VelocityAndShoot(float inputButton)
    {
        if (_isPressed && inputButton < 0.1)
        {
            // Stop to press ==> shoot
            ShotSnowBall();
        }
        else if (_isPressed && inputButton >= 0.1)
        {
            // Continue to press ==> increase velocity
            IncreaseVelocity(inputButton);
        }
        else if (!_isPressed && inputButton >= 0.1)
        {
            // Start to press ==> init to increase velocity
            _isPressed = true;
            _timePressed = Time.time;
            _currentVelocity = 0.0f;
            _velocityDirection = 1;
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

    public void ShotSnowBall() {
	    var snowBall = InstantiateSnowBall();
	    ISnowBallController ballController = snowBall.GetComponent<ISnowBallController>();

	    var shootAngle = GetShootAngle();
	    ballController?.SetVelocity(GetVelocityVector(shootAngle), shootAngle, _snowBallSpawn.forward);
	    _isPressed = false;
    }

    public abstract GameObject InstantiateSnowBall();
}
