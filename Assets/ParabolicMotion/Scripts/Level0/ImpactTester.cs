using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactTester : MonoBehaviour {

	public AudioSource audioSource;
	private bool _wasReleased = false;

	public void ReleaseObject() {
		_wasReleased = true;
		Level0Manager.instance.SetLastReleaseTimeAndHeight(Time.time, transform.position.y);
		audioSource.Play();
	}

	private void OnCollisionEnter(Collision other) {
		if (_wasReleased) {
			Level0Manager.instance.SetLastImpactTime(Time.time);
			audioSource.Stop();
		}
	}
}
