using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Ball : NetworkBehaviour {

    private Rigidbody rigidBody = null;
    private Vector3 startingPosition = Vector3.zero;
    float minimumVelocity = 5;
    float maximumVelocity = 200;

    // Setting up an event system so that score logic can be contained to a separate script.
    // https://unity3d.com/learn/tutorials/topics/scripting/events
    public delegate void BallEventHandler();
    public static event BallEventHandler OnTriggerEnterGoal1;
    public static event BallEventHandler OnTriggerEnterGoal2;

    // Use this for initialization
    void Start () {
        if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

        rigidBody = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

        if (rigidBody.velocity.magnitude < minimumVelocity)
        {
            //added a ver small number to keep from dividing by zero
            rigidBody.velocity *= minimumVelocity / rigidBody.velocity.magnitude + 0.000000001F;
        }
        if (rigidBody.velocity.magnitude > maximumVelocity)
        {
            //added a ver small number to keep from dividing by zero
            rigidBody.velocity *= maximumVelocity / rigidBody.velocity.magnitude + 0.000000001F;
        }
    }

    public void ClampSpeed(float min, float max)
    {
        if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;
        minimumVelocity = min;
        maximumVelocity = max;

    }

    public void UnClampSpeed()
    {
        minimumVelocity = 5;
        maximumVelocity = 200;
    }

    public void ResetPosition()
    {
        if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

        rigidBody.velocity = Vector3.zero;
        transform.forward = new Vector3(1, 0, 1);
        transform.position = startingPosition;
    }

    void OnTriggerEnter(Collider Col)
	{
		// Only handle scoring server-side of local
		if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

        // If the ball collided with Goal1 or Goal2:
        if (Col.gameObject.tag == "Goal1" || Col.gameObject.tag == "Goal2")
        {
            // If the ball collided with Goal1:
            if (Col.gameObject.tag == "Goal1")
            {
                //Debug.Log("Collided with Goal1");

                // Alert other scripts that ball hit Goal1.
                if (OnTriggerEnterGoal1 != null)
                {
                    OnTriggerEnterGoal1();
                }
                    
            }
            // Else if the ball collided with Goal2:
            else if (Col.gameObject.tag == "Goal2")
            {
                //Debug.Log("Collided with Goal2");

                // Alert other scripts that ball hit Goal2.
                if (OnTriggerEnterGoal2 != null)
                {
                    OnTriggerEnterGoal2();
                }
            }

            // We need to add some wait time and "Goal!" message eventually. The following would not work without other changes:
            //yield return new WaitForSeconds(5);

            // Respawn ball at center if on the server or local
            ResetPosition();
        }
    }

	void OnCollisionExit(Collision other)
	{
        // Only handle ball force server-side of local
        if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

		if (other.gameObject.tag == "Paddle" ||
		   	other.gameObject.tag == "Bumper")
		{
			Debug.Log ("Adding force to ball");
			rigidBody.AddForce (rigidBody.velocity.normalized * 2.5f);
		}
	}
}
