using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Actualiza las posiciones del LineRenderer de la cuerda
public class SlingCord : MonoBehaviour
{
	public Transform attachPoint;	//Punto donde va atada la cuerda (el otro punto es el propio componente)
	LineRenderer lineRenderer;

	private void Start()
	{
		//Obtiene la referencia
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Update ()
	{
		//Actualiza con las nuevas posiciones
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, attachPoint.position);
	}
}
