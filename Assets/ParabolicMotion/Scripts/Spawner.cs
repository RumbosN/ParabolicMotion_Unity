using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject prefabToSpawn;
	public float areaRadius = 0.2f;

	GameObject current;

	void Start()
	{
		DoSpawn();
	}

	void Update()
	{
		// If there are an object, check the distance to the area to create a new object.
		if (current != null)
		{
			Vector3 distance = current.transform.position - transform.position;

			if (distance.magnitude > areaRadius)
            {
				DoSpawn();
            }
		}
	}

	void DoSpawn()
	{
		if (prefabToSpawn != null)
        {
			current = GameObject.Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
		}
	}
}
