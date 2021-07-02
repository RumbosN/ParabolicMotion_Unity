using UnityEngine;

[RequireComponent(typeof(Handeable))]
public class Highlight : MonoBehaviour
{
	public Material hoverMaterial;
	private Material defaultMaterial;
	private Renderer rendered;
	private int handsCounter = 0;

	void Start() {
		rendered = GetComponent<Renderer>();
	}

	public void OnHoverBegin(HandInteraction hand) {
		if (handsCounter == 0) {
			defaultMaterial = rendered.sharedMaterial;
			rendered.sharedMaterial = hoverMaterial;
		}

		handsCounter++;
	}

	public void OnHoverUpdate(HandInteraction hand) {

	}

	public void OnHoverEnd(HandInteraction hand) {
		handsCounter--;

		if (handsCounter == 0) {
			rendered.sharedMaterial = defaultMaterial;
		}
	}
}
