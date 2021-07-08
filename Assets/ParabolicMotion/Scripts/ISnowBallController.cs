using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISnowBallController {

	void SetVelocity(Vector3 velocity, float? shootAngle, Vector3? forward);
	void DontDespawn();
	bool IsMine();
}
