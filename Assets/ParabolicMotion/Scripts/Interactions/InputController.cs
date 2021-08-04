using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

	[SerializeField] private KeyboardController keyboardListen;
	[SerializeField] private Color onColor;
	[SerializeField] private Color offColor;
	[SerializeField] private Color disabledColor;

	private TMP_InputField textMesh;
	private Image image;
	private bool isDisabled = false;

	public string Text => textMesh.text;

	void Start() {
		textMesh = GetComponent<TMP_InputField>();
		image = GetComponent<Image>();
	}

	public void Hover() {
		if (isDisabled) {
			return;
		}

		image.color = onColor;
	}

	public void UnHover() {
		if (isDisabled) {
			return;
		}

		image.color = offColor;
	}

	public void AppendText(string text) {
		if (isDisabled) {
			return;
		}

		textMesh.text += text;
	}

	public void SetText(string text) {
		if (isDisabled) {
			return;
		}

		textMesh.text = text;
	}

	public void RemoveLast() {
		if (isDisabled) {
			return;
		}

		var last = textMesh.text.Length;
		if (last > 0) {
			textMesh.text = textMesh.text.Remove(last - 1);
		}
	}

	public void OnTriggerExit(Collider col) {
		if (isDisabled) {
			return;
		}

		if (col.gameObject.layer == Constants.KeyboardInteractiveLayer) {
			keyboardListen.SetInputText(this);
		}
	}

	public void SetDisable(bool disabled) {
		isDisabled = disabled;
		textMesh.text = "";
		image.color = disabled ? disabledColor : offColor;
	}
}
