using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Base class for paddles
/// </summary>

public class PaddleBase : NetworkBehaviour {
	
    private float thrust = 10.0f;

    private Vector3 up = new Vector3(1, 0, 0);

    private Rigidbody rigidBody;

	
	public virtual void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
	}


	protected void SetThrust(float thrust)
	{
		this.thrust = thrust;
	}

	// +ve for "up"
    protected void MovePaddles(float dir)
    {
		Debug.Assert(Time.inFixedTimeStep, "Paddle movement should only happen inside physics code");

		if (rigidBody)
        {
			// Clamp between [-1,1]
			dir = Mathf.Clamp(dir, -1.0f, 1.0f);

			//rigidBody.AddForce(dir * up * Thrust);
			transform.Translate(dir * up * thrust * Time.fixedDeltaTime);
		}
    }
}
