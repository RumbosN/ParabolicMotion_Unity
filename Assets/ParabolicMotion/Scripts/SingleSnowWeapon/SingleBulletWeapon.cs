using UnityEngine;

public class SingleBulletWeapon : BulletWeapon
{
	public override GameObject InstantiateSnowBall() {
		return Instantiate(_projectilePrefab, _projectileSpawn.position, Quaternion.Euler(Vector3.zero));
    }
}
