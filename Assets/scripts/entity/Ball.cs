using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour {

    private Rigidbody rb = null;
    private Vector3 startingPosition = Vector3.zero;
    float minimumVelocity = 5;
    float startingMinVelocity = -10;
    float startingMaxVelocity = 10;

    // Use this for initialization
    void Start () {
        if (!isServer) return;

        rb = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (!isServer) return;

        if (rb.velocity.magnitude < minimumVelocity)
        {
            rb.velocity = rb.velocity * 1.25f;
        }
    }

    public void ResetPosition()
    {
        if (!isServer) return;

        transform.position = startingPosition;
    }
}
