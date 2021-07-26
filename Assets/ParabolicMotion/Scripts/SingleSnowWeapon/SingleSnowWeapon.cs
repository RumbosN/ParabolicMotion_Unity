using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SingleSnowWeapon : SnowWeapon
{
	public override GameObject InstantiateSnowBall() {
		return Instantiate(_snowBallPrefab, _snowBallSpawn.position, _transform.rotation);
    }
}
