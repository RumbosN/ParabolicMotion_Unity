using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SingleBulletController : MonoBehaviour, IBulletController {

    protected ParabolicMotionPhysics _parabolicMotionPhysics;
    [SerializeField] protected bool _shouldDespawn = true;


    private void Awake()
    {
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
        Despawn();
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(Constants.TimeToSpawnSnowBall);
        Despawn();
    }

    public void Despawn()
    {
        if (_shouldDespawn)
        {
            LevelManager.instance.RemoveBullet(gameObject);
            Destroy(gameObject);
        }
    }
}
