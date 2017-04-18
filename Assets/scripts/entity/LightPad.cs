using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lights up when a ball triggers it
/// </summary>
public class LightPad : MonoBehaviour {

	public Color baseColour;
	public Color lightColour;
	private int balls;

	void Start () 
	{
		balls = 0;
		SetColor(baseColour);
	}

	void Update () 
	{
		
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Ball")
		{
			balls += 1;

			if (balls > 0)
			{
				SetColor(lightColour);
			}
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "Ball")
		{
			balls -= 1;

			if (balls <= 0)
			{
				SetColor(baseColour);
			}
		}
	}

	void SetColor(Color color)
	{
		GetComponent<Renderer>().material.color = color;
		GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
	}
}
