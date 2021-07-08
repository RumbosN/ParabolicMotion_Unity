using UnityEngine;

public class Handeable : MonoBehaviour
{
	public EHandPose poseName;
	public Transform attachPointRight;
	public Transform attachPointLeft;

	public bool receiveHoverEvents = true;

	// The hand should be free to handle the object
	public bool exclusiveHover = true;
}
