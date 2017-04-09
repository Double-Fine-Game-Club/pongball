using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controlled paddle
/// </summary>

public class Player : PaddleBase {

	// Use this for initialization
	public override void Start()
    {
		base.Start();
		
		// Give the player faster movement
		SetThrust(20);
    }

    private void FixedUpdate()
	{
		float haxis = Input.GetAxis("Horizontal");
		float vaxis = Input.GetAxis("Vertical");

		if (haxis != 0)
		{

		}

		if (vaxis != 0)
		{
			vaxis *= -1;
			//rigidBody.AddForce(up * (Thrust * vaxis));
			//transform.Translate(up * (Thrust * vaxis) * Time.deltaTime);
			MovePaddles(vaxis);
        }
	}
}
