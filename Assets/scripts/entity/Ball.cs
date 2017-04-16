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
}
