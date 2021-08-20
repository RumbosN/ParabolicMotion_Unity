using System;
using UnityEngine;

[System.Serializable]
public struct ParabolicVariables {

	public ParabolicVariables(float v, float alpha, float beta, Vector3 otherLandPosition, float tableLocalY, float distance) {
		this.v = v;
		this.alpha = alpha;
		this.beta = beta;
		this.otherLandPosition = otherLandPosition;
		this.tableLocalY = tableLocalY;
		this.distance = distance;
	}

	public float v;
	public float alpha;
	public float beta;
	public Vector3 otherLandPosition;
	public float tableLocalY;
	public float distance;
}
