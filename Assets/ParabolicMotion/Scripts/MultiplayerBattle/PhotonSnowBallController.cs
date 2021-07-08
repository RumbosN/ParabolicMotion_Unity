﻿using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PhotonSnowBallController : MonoBehaviourPun, IPunObservable, ISnowBallController
{
    protected Rigidbody _rb;
    protected BattleGameManager _battleGameManager;
    protected Transform _transform;
    protected bool _shouldDespawn;

    private void Awake()
    {
        _battleGameManager = BattleGameManager.instance;
        _shouldDespawn = true;
        _rb = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();

        if (!photonView.IsMine)
        {
            _rb.isKinematic = true;
            _shouldDespawn = false;
        }

    }

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            _shouldDespawn = true;
            StartCoroutine(DespawnTimer());
        }
    }

    public void SetVelocity(Vector3 velocity, float? shootAngle, Vector3? forward)
    {
        if (photonView.IsMine)
        {
            _rb.velocity = velocity;
        }
    }

    public void DontDespawn()
    {
        if (photonView.IsMine)
        {
            _shouldDespawn = false;
        }
    }

    public bool IsMine() {
	    return photonView != null && photonView.IsMine;
    }

    public void SetShootAngle(float angle) {
    }

    public void SetForward(Vector3 forward) {
    }

    private void OnCollisionEnter(Collision other)
    {
        if (photonView.IsMine)
        {
            Despawn();
        }
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(_battleGameManager.TimeToSpawnSnowBall);
        Despawn();
    }

    private void Despawn()
    {
        if (_shouldDespawn && photonView.IsMine)
        {
            Destroy();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
           stream.SendNext(_rb.velocity);
        }

        if (stream.IsReading)
        {
            _rb.velocity = (Vector3)stream.ReceiveNext();
        }
    }

    private void Destroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
