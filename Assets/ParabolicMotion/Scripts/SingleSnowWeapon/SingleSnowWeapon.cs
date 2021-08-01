using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SingleSnowWeapon : SnowWeapon
{
	public override GameObject InstantiateSnowBall() {
		return Instantiate(_projectilePrefab, _projectileSpawn.position, Quaternion.Euler(Vector3.zero));
    }
}
