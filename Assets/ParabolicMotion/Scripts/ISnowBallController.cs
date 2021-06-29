using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISnowBallController {

	void SetVelocity(Vector3 velocity);
	void DontDespawn();
	bool IsMine();
	void SetShootAngle(float angle);
	void SetForward(Vector3 forward);

}
