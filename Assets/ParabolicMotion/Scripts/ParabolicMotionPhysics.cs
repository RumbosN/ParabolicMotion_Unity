using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParabolicMotionPhysics : MonoBehaviour
{
	[SerializeField] protected float _impactRadius = 0.1f;
	[SerializeField] protected LayerMask _impactMask;
	[SerializeField] protected bool _callEventsWithImpact = true;

	protected IBulletController _bullet;

	protected Vector3 _initialVelocity;
	protected Transform _transform;

	protected float Voy;
	protected float Vox;
	protected float alpha;
	protected Vector3 fw;

	protected float _g;
	protected bool _isMoving;

	private void Awake() {
		_transform = GetComponent<Transform>();
		_g = Constants.Gravity;
		_bullet = GetComponent<IBulletController>();
		_isMoving = false;
	}

	private void Update() {
		if (_isMoving) {
			var colliders = Physics.OverlapSphere(transform.position, _impactRadius, _impactMask);

			if (colliders.Length == 0) {
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
			else {
				StopMovement();

				if (_callEventsWithImpact) {
					if (colliders.First().CompareTag(Constants.GoalTag)) {
						LevelManager.instance?.EventsResponse.successfulResponseEvent?.Invoke();
					}
					else {
						LevelManager.instance?.EventsResponse.failedResponseEvent?.Invoke();
					}
				}
			}
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
		_bullet.Despawn();
	}
}
