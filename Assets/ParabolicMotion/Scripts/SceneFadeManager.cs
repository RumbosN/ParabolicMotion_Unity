using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFadeManager : Singleton<SceneFadeManager> {

	[SerializeField] private OVRScreenFade OVRScreenFade;
	private float fadeTime;

	void Start() {
		fadeTime = OVRScreenFade.fadeTime;
	}

	public void FadeOut(int secondsToFadeIn) {
		OVRScreenFade.FadeOut();
		StartCoroutine(FadeIn(secondsToFadeIn));
	}

	private IEnumerator FadeIn(int secondsToFadeIn) {
		yield return new WaitForSeconds(fadeTime + secondsToFadeIn);
		OVRScreenFade.SetUIFade(-1.0f);
		StartCoroutine(Fade(1, 0));
	}

	IEnumerator Fade(float startAlpha, float endAlpha) {
		float elapsedTime = 0.0f;
		while (elapsedTime < fadeTime) {
			elapsedTime += Time.deltaTime;
			OVRScreenFade.SetFadeLevel(Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTime)));
			yield return new WaitForEndOfFrame();
		}
	}
}
