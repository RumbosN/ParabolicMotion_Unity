using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactTester : MonoBehaviour {

	public AudioSource audioSource;
	private bool _wasReleased = false;
	protected ParabolicMotionPhysics _parabolicMotionPhysics;

	void Awake() {
		_parabolicMotionPhysics = GetComponent<ParabolicMotionPhysics>();
	}

	public void ReleaseObject() {
		_wasReleased = true;
		Level0Manager.instance.SetLastReleaseTimeAndHeight(Time.time, transform.position.y);
		_parabolicMotionPhysics.SetupMovement(Vector3.zero, 0.0f, Vector3.zero);
		audioSource.Play();
	}

	private void OnCollisionEnter(Collision other) {
		if (_wasReleased) {
			Level0Manager.instance.SetLastImpactTime(Time.time);
			_parabolicMotionPhysics.StopMovement();
			audioSource.Stop();
		}
	}
}
