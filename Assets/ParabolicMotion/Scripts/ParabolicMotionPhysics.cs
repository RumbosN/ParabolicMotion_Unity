using System;
using System.Linq;
using UnityEngine;

public abstract class ParabolicMotionPhysics : MonoBehaviour
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

	protected void Awake()
	{
		_transform = GetComponent<Transform>();
		_g = Constants.Gravity;
		_bullet = GetComponent<IBulletController>();
		_isMoving = false;
	}

	protected void Update() {
		if (_isMoving) {
			float t = Time.deltaTime;
			var updatePositionResponse = UpdatePosition(t);
			CheckImpact(updatePositionResponse.newPosition, updatePositionResponse.newVy);
		}
	}

	public void SetupMovement(Vector3 velocity, float shootAngle, Vector3 forward)
	{
		_initialVelocity = velocity;
		Voy = (float)(Math.Sin(shootAngle) * velocity.magnitude);
		Vox = (float)(Math.Cos(shootAngle) * velocity.magnitude);
		alpha = shootAngle;
		fw = forward;
		_isMoving = true;

		GetComponent<Rigidbody>().isKinematic = true;
	}

	public void StopMovement()
	{
		_isMoving = false;
		_bullet.Despawn();
	}

	protected EBulletOverlapResponse GetImpactResponse() {
		//var impactRadius = (_transform.position - newPosition).magnitude + _myRadius;
		var colliders = Physics.OverlapSphere(transform.position, _impactRadius, _impactMask);

		if (colliders.Length == 0) {
			return EBulletOverlapResponse.NOTHING;
		}

		if (colliders.First().CompareTag(Constants.GoalTag)) {
			return EBulletOverlapResponse.GOAL;
		}

		return EBulletOverlapResponse.FAIL;
	}

	protected void CallImpactEvent(EBulletOverlapResponse overlapResponse) {
		if (_callEventsWithImpact) {
			if (overlapResponse == EBulletOverlapResponse.GOAL) {
				//LevelManager.instance?.EventsResponse.successfulResponseEvent?.Invoke();
			}
			else {
				LevelManager.instance?.EventsResponse.failedResponseEvent?.Invoke();
			}
		}
	}

	protected void CheckImpact(Vector3 newPosition, float newVy) {
		var impactResponse = GetImpactResponse();

		if (impactResponse == EBulletOverlapResponse.NOTHING) {
			_transform.position = newPosition;
			Voy = newVy;
		}
		else {
			StopMovement();
			CallImpactEvent(impactResponse);
		}
	}

	protected abstract UpdatePositionResponse UpdatePosition(float t);

#region ProtectedStructures
	protected struct UpdatePositionResponse {

		public Vector3 newPosition;
		public float newVy;

		public UpdatePositionResponse(Vector3 newPosition, float newVy) {
			this.newPosition = newPosition;
			this.newVy = newVy;
		}
	}

	protected enum EBulletOverlapResponse {
		NOTHING,
		GOAL,
		FAIL
	}
#endregion
}
