using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PhotonBulletWeapon : BulletWeapon
{

	[SerializeField] protected EPlayerId _playerId;
	public EPlayerId PlayerId => _playerId;


    public override GameObject InstantiateSnowBall()
    {
        return PhotonNetwork.Instantiate(_projectilePrefab.name, _projectileSpawn.position, _transform.rotation);
    }
}
