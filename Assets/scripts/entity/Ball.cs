using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour {

    private Rigidbody rigidBody = null;
    private Vector3 startingPosition = Vector3.zero;
    float minimumVelocity = 5;



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
            rigidBody.velocity = rigidBody.velocity * 1.25f;
        }
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
        if (Col.gameObject.tag == "Goal1" || Col.gameObject.tag == "Goal2")
        {
            if (Col.gameObject.tag == "Goal1")
            {
                Debug.Log("Collided with Goal1");
            }
            else
            {
                Debug.Log("Collided with Goal2");
            }

            //yield return new WaitForSeconds(5); // We need to add some sort of wait time and "Goal!" text eventually.
            ResetPosition();
        }
    }
}
