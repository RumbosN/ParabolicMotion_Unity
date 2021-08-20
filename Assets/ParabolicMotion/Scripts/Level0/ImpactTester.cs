using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParabolicMotionPhysics))]
public class ImpactTester : MonoBehaviour, IBulletController {

	public AudioSource audioSource;
	private bool _wasReleased = false;
	protected ParabolicMotion2DEngine _parabolicMotionPhysics;

	void Awake() {
		_parabolicMotionPhysics = GetComponent<ParabolicMotion2DEngine>();
	}

	public void ReleaseObject() {
		_wasReleased = true;
		Level0Manager.instance.SetLastReleaseTimeAndHeight(Time.time, transform.position.y);
		_parabolicMotionPhysics.SetupMovement(Vector3.zero, 0.0f, Vector3.zero);
		audioSource.Play();
	}

	#region BulletController
	public void SetVelocity(Vector3 velocity, float? shootAngle, Vector3? forward) {
	}

	public void DontDespawn() {
	}

	public bool IsMine() {
		return true;
	}

	public void Despawn() {
		if (_wasReleased) {
			Level0Manager.instance.SetLastImpactTime(Time.time);
			audioSource.Stop();
			GetComponent<Rigidbody>().isKinematic = false;
		}
	}
	#endregion


}
