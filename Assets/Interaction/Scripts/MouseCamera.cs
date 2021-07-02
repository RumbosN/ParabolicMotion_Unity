using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamera : MonoBehaviour
{	
	public float speed = 3;

	Vector2 rotation = new Vector2(0, 0);
	Vector3 rot = new Vector3(0, 0);

	void Update()
	{
		rotation.y += Input.GetAxis("Mouse X");
		rotation.x += -Input.GetAxis("Mouse Y");

		//rot.x += -Input.GetAxis("Mouse Y");
		//rot.y += Input.GetAxis("Mouse X");
		//transform.eulerAngles )

		Vector3 rotateInput = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f);
		transform.eulerAngles = transform.eulerAngles + rotateInput * speed;

		//transform.Rotate(rotateInput * speed, Space.World);

		//transform.eulerAngles = (Vector2)rotation * speed;
	}	
}
