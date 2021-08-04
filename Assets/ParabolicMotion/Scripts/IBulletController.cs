using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletController {

	void SetVelocity(Vector3 velocity, float? shootAngle, Vector3? forward);
	void DontDespawn();
	bool IsMine();
	void Despawn();
}
