using System;
using UnityEngine;

[System.Serializable]
public struct ParabolicVariables {

	public ParabolicVariables(float v, float alpha, Vector3 otherLandPosition, float tableLocalY, float distance) {
		this.v = v;
		this.alpha = alpha;
		this.otherLandPosition = otherLandPosition;
		this.tableLocalY = tableLocalY;
		this.distance = distance;
	}

	public float v;
	public float alpha;
	public Vector3 otherLandPosition;
	public float tableLocalY;
	public float distance;
}
