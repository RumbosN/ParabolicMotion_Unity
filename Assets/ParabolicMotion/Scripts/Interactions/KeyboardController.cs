using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {

	[SerializeField] private GameObject textError;
	[SerializeField] private int waitingTimeToHideErrorText;

	private InputController currentInputText;

	void Start() {
		textError.SetActive(false);
	}

	public void SetInputText(InputController input) {
		currentInputText?.UnHover();
		currentInputText = input;
		currentInputText.Hover();
	}

	public void AppendTextToInput(string text) {
		if (currentInputText != null) {
			currentInputText.AppendText(text);
		}
		else {
			ShowError();
		}
	}

	public void ReplaceTextToInput(string text) {
		if (currentInputText != null) {
			currentInputText.SetText(text);
		}
		else {
			ShowError();
		}
	}

	public void RemoveLastToInput() {
		currentInputText?.RemoveLast();
	}

	private void ShowError() {
		textError.SetActive(true);
		StartCoroutine(HideError());
	}

	private IEnumerator HideError() {
		yield return new WaitForSeconds(waitingTimeToHideErrorText);
		textError.SetActive(false);
	}

	public void ReleaseCurrentInput()
    {
		if (currentInputText != null)
		{
			currentInputText.UnHover();
			currentInputText = null;
		}
	}
}
