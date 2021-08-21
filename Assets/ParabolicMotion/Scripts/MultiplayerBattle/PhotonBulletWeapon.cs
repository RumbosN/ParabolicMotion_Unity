using Photon.Pun;
using UnityEngine;

public class PhotonBulletWeapon : BulletWeapon
{

	[SerializeField] protected EPlayerId _playerId;
	public EPlayerId PlayerId => _playerId;


    public override GameObject InstantiateSnowBall()
    {
        return PhotonNetwork.Instantiate(_projectilePrefab.name, _projectileSpawn.position, _transform.rotation);
    }
}
