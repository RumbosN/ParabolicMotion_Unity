using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

		UnselectKey();
	}

	public void ReplaceTextToInput(string text) {
		if (currentInputText != null) {
			currentInputText.SetText(text);
		}
		else {
			ShowError();
		}

		UnselectKey();
	}

	public void RemoveLastToInput() {
		currentInputText?.RemoveLast();
		UnselectKey();
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

	public void UnselectKey() {
		EventSystem.current.SetSelectedGameObject(null);
	}
}
