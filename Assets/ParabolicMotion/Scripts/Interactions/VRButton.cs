using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class VRButton : MonoBehaviour
{
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    private Image image;
    private Button button;

    void Start() {
	    image = GetComponent<Image>();
	    button = GetComponent<Button>();
    }

    public void OnTriggerEnter(Collider col) {
	    if (col.gameObject.layer == Constants.KeyboardInteractiveLayer) {
			image.color = onColor;
	    }
    }

    public void OnTriggerExit(Collider col) {
	    if (col.gameObject.layer == Constants.KeyboardInteractiveLayer) {
		    button.onClick.Invoke();
		    image.color = offColor;
	    }
    }
}
