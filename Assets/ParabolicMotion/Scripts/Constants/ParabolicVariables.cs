using UnityEngine;

[System.Serializable]
public struct ParabolicVariables {

	public ParabolicVariables(float v, float alpha, Vector3 otherLandPosition, float tableLocalY) {
		this.v = v;
		this.alpha = alpha;
		this.otherLandPosition = otherLandPosition;
		this.tableLocalY = tableLocalY;
	}

	public float v;
	public float alpha;
	public Vector3 otherLandPosition;
	public float tableLocalY;
}
