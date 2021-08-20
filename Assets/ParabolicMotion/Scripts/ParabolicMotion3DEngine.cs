using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicMotion3DEngine : ParabolicMotionPhysics
{
    protected override UpdatePositionResponse UpdatePosition(float t)
    {
		var Vy = Voy + _g * t;
		var direction = new Vector3(fw.x / Math.Abs(fw.x), 1.0f, fw.z / Math.Abs(fw.z));
		var velocity = new Vector3(_initialVelocity.x * t, (Vy + 0.5f * _g * t) * t, _initialVelocity.z * t);
		var newPosition = _transform.position + Vector3.Scale(direction, velocity);

		return new UpdatePositionResponse(newPosition, Vy);
	}
}
