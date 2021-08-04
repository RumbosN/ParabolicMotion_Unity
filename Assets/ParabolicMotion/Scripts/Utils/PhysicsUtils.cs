using System;
using UnityEngine;

public static class PhysicsUtils
{
	public static Vector2 DecomposedVelocity(float v, float alpha) {
		return new Vector2(v * (float) Math.Cos(Mathf.Deg2Rad * alpha), v * (float) Math.Sin(Mathf.Deg2Rad * alpha));
	}
}
