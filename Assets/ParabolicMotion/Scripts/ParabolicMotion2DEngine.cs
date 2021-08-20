using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParabolicMotion2DEngine : ParabolicMotionPhysics
{
    protected override UpdatePositionResponse UpdatePosition(float t)
    {
		Vector3 currentX = new Vector3(_transform.position.x, 0.0f, _transform.position.z);
		Vector3 fwX = new Vector3(fw.x, 0.0f, fw.z).normalized;
		Vector3 X = currentX + (fwX * Vox * t);

		Vector3 currentY = new Vector3(0.0f, _transform.position.y, 0.0f);
		var Vy = Voy + _g * t;
		Vector3 Y = currentY + (Vector3.up * Vy * t) + (Vector3.up * 0.5f * _g * t * t);
		var newPosition = X + Y;

		return new UpdatePositionResponse(newPosition, Vy);
	}
}
