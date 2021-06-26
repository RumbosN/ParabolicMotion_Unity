using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SnowWeapon : MonoBehaviourPun
{
    [SerializeField] private EPlayerId _playerId;
    [SerializeField] private GameObject _snowBallPrefab;
    [SerializeField] private Transform _snowBallSpawn;
    [SerializeField] private float _maximumHorizontalAngle = 45f;
    [SerializeField] private float _maximumVerticalAngle = 60f;
    private float _simulationRate = 60f;
    private float _rotationScaleMultiplier = 1.0f;
    
    private bool _isPressed;
    public float maxVelocity = 60.0f;
    public float _velocityIncreaseRate = 5.0f;
    private float _currentVelocity = 0.0f;
    private float _timePressed;
    private int _velocityDirection = 1;
    
    public float rotationAmount = 1.5f;
    public EPlayerId PlayerId => _playerId;
    public float CurrentVelocity => _currentVelocity;

    protected Transform _transform;
    
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _isPressed = false;
    }

    public void Rotate(Vector2 axisRotation)
    {
        Vector3 euler = _transform.rotation.eulerAngles;
        float rotateInfluence = _simulationRate * Time.deltaTime * rotationAmount * _rotationScaleMultiplier;

        var newY = euler.y + axisRotation.x * rotateInfluence;
        if (Math.Abs(newY) <= _maximumHorizontalAngle || 
            Math.Abs(newY) >= 360 - _maximumHorizontalAngle)
        {
            euler.y = newY;
        }
        
        var newX = euler.x - axisRotation.y * rotateInfluence;
        if (Math.Abs(newX) <= _maximumHorizontalAngle || 
            Math.Abs(newX) >= 360 - _maximumHorizontalAngle)
        {
            euler.x = newX;
        }
        
        _transform.rotation = Quaternion.Euler(euler);
    }

    public void Rotate(Vector3 newRotationEuler)
    {
        Vector3 euler = _transform.rotation.eulerAngles;
        euler.x = newRotationEuler.x;
        euler.y = newRotationEuler.y;
        _transform.rotation = Quaternion.Euler(euler);
    }

    public void ShotSnowBall()
    {
        var snowBall = PhotonNetwork.Instantiate(_snowBallPrefab.name, _snowBallSpawn.position, _transform.rotation);
        var ballController = snowBall.GetComponent<SnowBallController>();

        if (ballController != null)
        {
            ballController.SetVelocity(GetVelocityVector());
        }

        _isPressed = false;
    }

    private Vector3 GetVelocityVector()
    {
        var alpha = GetShootAngle();
        var fw = _transform.forward;

        var vY = Mathf.Abs(_currentVelocity * Mathf.Sin(alpha));
        var vXUser = _currentVelocity * Mathf.Cos(alpha);
            
        var tita = Mathf.Atan(fw.x / fw.z);
        var vX = Mathf.Abs(vXUser * Mathf.Sin(tita));
        var vZ = Mathf.Abs(vXUser * Mathf.Cos(tita));
            
        return new Vector3(fw.x * vX, fw.y * vY, fw.z * vZ);
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

    private void IncreaseVelocity(float inputButton)
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
}
