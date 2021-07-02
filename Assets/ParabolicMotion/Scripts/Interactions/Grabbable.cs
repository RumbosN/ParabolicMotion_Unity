using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour
{
	public EButtonInputController grabButton;
	public UnityEvent releasedEvent;
	public UnityEvent attachedEvent;
	private HandInteraction attachedHand;
	private Dictionary<EButtonInputController, Dictionary<EHandSide, OVRInput.Button>> ButtonInputControllerMapper = InputControllerMapper.ButtonInputControllerMapper;

	public void OnHoverUpdate(HandInteraction hand) {
		if (OVRInput.Get(ButtonInputControllerMapper[grabButton][hand.handSource])) {
			if (attachedHand != null && attachedHand != hand) {
				Debug.Log("Grabbable: OnHoverUpdate - Release");
				attachedHand.Release();
			}

			hand.Attach(GetComponent<Handeable>());
			attachedEvent?.Invoke();
			attachedHand = hand;
		}
	}

	public void OnAttachUpdate(HandInteraction hand) {
		if (!OVRInput.Get(ButtonInputControllerMapper[grabButton][hand.handSource])) {
			Debug.Log("Grabbable: OnAttachUpdate - Release");
			hand.Release();
			releasedEvent?.Invoke();
		}
	}

	public void OnAttachEnd(HandInteraction hand) {
		attachedHand = null;

		var rb = GetComponent<Rigidbody>();

		if (rb != null) {
			// Get hand velocity to set the velocity to hand velocity.
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
	}
}
