using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour {

    private Rigidbody rb = null;
    private Vector3 startingPosition = Vector3.zero;
    float minimumVelocity = 3;
    float startingMinVelocity = -10;
    float startingMaxVelocity = 10;

    // Use this for initialization
    void Start () {
        if (!isServer) return;

        rb = GetComponent<Rigidbody>();
        startingPosition = transform.position;

        LaunchBall();
    }

    private void LaunchBall()
    {
        /*
        transform.position = startingPosition;

        float startingXForce = UnityEngine.Random.Range(startingMinVelocity, startingMaxVelocity);
        float startingZForce = UnityEngine.Random.Range(startingMinVelocity, startingMaxVelocity);

        if (startingXForce * startingZForce < 50)
        {
            startingXForce = startingMaxVelocity;
        }

        rb.AddForce(new Vector3(startingXForce, 0, startingZForce));
        */
    }

    private void FixedUpdate()
    {
        if (!isServer) return;

        if (rb.velocity.magnitude < minimumVelocity)
        {
            rb.AddForce(rb.velocity.normalized * minimumVelocity);
        }
    }

    public void ResetPosition()
    {
        if (!isServer) return;

        transform.position = startingPosition;
    }
}
