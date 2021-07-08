using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SingleSnowBallController : MonoBehaviour, ISnowBallController {

    protected BattleGameManager _battleGameManager;
    protected ParabolicMotionPhysics _parabolicMotionPhysics;
    [SerializeField] protected bool _shouldDespawn = true;


    private void Awake()
    {
        _battleGameManager = BattleGameManager.instance;
        _parabolicMotionPhysics = GetComponent<ParabolicMotionPhysics>();
    }

	private void OnEnable()
    {
        StartCoroutine(DespawnTimer());
    }

    public void SetVelocity(Vector3 velocity, float? shootAngle, Vector3? forward)
    {
	    if (forward == null || shootAngle == null) {
		    throw new ArgumentException("forward and shoot angle cannot be null");
	    }

	    _parabolicMotionPhysics.SetupMovement(velocity, (float)shootAngle, (Vector3)forward);
    }

    public void DontDespawn()
    {
        _shouldDespawn = false;
    }

    public bool IsMine()
    {
	    return true;
    }

    private void OnCollisionEnter(Collision other)
    {
	    _parabolicMotionPhysics.StopMovement();
        Despawn();
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(_battleGameManager.TimeToSpawnSnowBall);
        Despawn();
    }

    private void Despawn()
    {
        if (_shouldDespawn)
        {
            Destroy(gameObject);
        }
    }
}
