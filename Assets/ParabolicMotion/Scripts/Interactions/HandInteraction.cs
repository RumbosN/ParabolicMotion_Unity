using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteraction : MonoBehaviour
{
	public float hoverRadius = 0.1f;
	public LayerMask hoverMask; // Layer of interactive objects
	public Animator animator;
	public EHandSide handSource;

	private Handeable hoverObj = null;
	private Handeable attachedObj = null;
	public Handeable AttachedObject => attachedObj;

	private FixedJoint joint;
	private string _poseAnimatorVariable = "pose";

	void Start() {
		joint = GetComponent<FixedJoint>();
		joint.autoConfigureConnectedAnchor = false;
	}

	void Update()
	{
		// Get interactive objects in a radius near the hand
		var colliders = Physics.OverlapSphere(transform.position, hoverRadius, hoverMask);

		float closestDistance = float.MaxValue;
		Handeable closestObj = null;

		foreach (var c in colliders)
		{
			var handeable = c.GetComponentInParent<Handeable>();

			if (handeable == null || !handeable.receiveHoverEvents || (handeable.exclusiveHover && attachedObj != null))
			{
				continue;
			}

			// c.ClosestPoint(transform.position) --> ask to collider which is the closest point to the hand
			float distance = (c.ClosestPoint(transform.position) - transform.position).magnitude;
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestObj = handeable;
			}
		}

		if (closestObj != hoverObj)
		{
			if (hoverObj != null)
			{
				hoverObj.SendMessage("OnHoverEnd", this, SendMessageOptions.DontRequireReceiver);
			}

			hoverObj = closestObj;
			if (hoverObj != null)
			{
				hoverObj.SendMessage("OnHoverBegin", this, SendMessageOptions.DontRequireReceiver);
			}
		}

		if (hoverObj != null)
		{
			hoverObj.SendMessage("OnHoverUpdate", this, SendMessageOptions.DontRequireReceiver);
		}

		if (attachedObj != null)
		{
			attachedObj.SendMessage("OnAttachUpdate", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void Attach(Handeable handeable)
	{
		Release(); // Only one object in the hand
		var rb = handeable.GetComponent<Rigidbody>();

		if (rb != null)
		{
			rb.transform.position = transform.position;
			rb.transform.rotation = transform.rotation;

			Transform attachReference = (handSource == EHandSide.LEFT)
				? handeable.attachPointLeft
				: handeable.attachPointRight;

			joint.anchor = joint.connectedAnchor = Vector3.zero; // Reset the previous
			if (attachReference != null)
			{
				joint.connectedAnchor = attachReference.localPosition;
				rb.transform.rotation *= Quaternion.Inverse(attachReference.localRotation);
			}

			joint.connectedBody = rb;
			attachedObj = handeable;
			attachedObj.SendMessage("OnAttachBegin", this, SendMessageOptions.DontRequireReceiver);

			animator?.SetInteger(_poseAnimatorVariable, (int)handeable.poseName);
		}
	}

	public void Release()
	{
		if (attachedObj != null)
		{
			attachedObj.SendMessage("OnAttachEnd", this, SendMessageOptions.DontRequireReceiver);
			joint.connectedBody = null;

			animator?.SetInteger(_poseAnimatorVariable, (int)EHandPose.Default);

			attachedObj = null;
		}
	}
}
