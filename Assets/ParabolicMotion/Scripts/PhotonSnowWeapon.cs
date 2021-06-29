using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PhotonSnowWeapon : SnowWeapon
{
    public override GameObject InstantiateSnowBall()
    {
        return PhotonNetwork.Instantiate(_snowBallPrefab.name, _snowBallSpawn.position, _transform.rotation);
    }
}
