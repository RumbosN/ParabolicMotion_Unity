using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

	[SerializeField] private KeyboardController keyboardListen;
	[SerializeField] private Color onColor;
	[SerializeField] private Color offColor;

	private TMP_InputField textMesh;
	private Image image;

	void Start() {
		textMesh = GetComponent<TMP_InputField>();
		image = GetComponent<Image>();
	}

	public void Hover() {
		image.color = onColor;
	}

	public void UnHover() {
		image.color = offColor;
	}

	public void AppendText(string text) {
		textMesh.text += text;
	}

	public void SetText(string text) {
		textMesh.text = text;
	}

	public void RemoveLast() {
		var last = textMesh.text.Length;
		if (last > 0) {
			textMesh.text = textMesh.text.Remove(last - 1);
		}
	}

	public void OnTriggerExit(Collider col) {
		if (col.gameObject.layer == Constants.KeyboardInteractiveLayer) {
			keyboardListen.SetInputText(this);
		}
	}
}
