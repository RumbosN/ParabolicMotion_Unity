using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParabolicMotionPhysics : MonoBehaviour
{

	protected Vector3 _initialVelocity;
	protected BattleGameManager _battleGameManager;
	protected Transform _transform;
	protected bool _isMoving = true;

	protected float Voy;
	protected float Vox;
	protected float alpha;
	protected Vector3 fw;

	protected float _g;

	private void Awake() {
		_transform = GetComponent<Transform>();
		_isMoving = false;
		_g = Constants.Gravity;
	}

	private void Update() {
		if (_isMoving ) {
			float t = Time.deltaTime;

			Vector3 currentX = new Vector3(_transform.position.x, 0.0f, _transform.position.z);
			Vector3 fwX = new Vector3(fw.x, 0.0f, fw.z).normalized;
			Vector3 X = currentX + (fwX * Vox * t);

			Vector3 currentY = new Vector3(0.0f, _transform.position.y, 0.0f);
			var Vy = Voy + _g * t;
			Vector3 Y = currentY + (Vector3.up * Vy * t) + (Vector3.up * 0.5f * _g * t * t);

			_transform.position = X + Y;

			Voy = Vy;
		}
	}

	public void SetupMovement(Vector3 velocity, float shootAngle, Vector3 forward) {
		_initialVelocity = velocity;
		Voy = (float) (Math.Sin(shootAngle) * velocity.magnitude);
		Vox = (float) (Math.Cos(shootAngle) * velocity.magnitude);
		alpha = shootAngle;
		fw = forward;

		_isMoving = true;
		GetComponent<Rigidbody>().isKinematic = true;
	}

	public void StopMovement() {
		_isMoving = false;
		GetComponent<Rigidbody>().isKinematic = false;
	}
}
