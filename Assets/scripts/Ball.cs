using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private Rigidbody rb = null;
    private Vector3 startingPosition = Vector3.zero;
    float minimumVelocity = 5;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        startingPosition = transform.position;

        LaunchBall();
    }

    private void LaunchBall()
    {
        transform.position = startingPosition;

        float startingXForce = UnityEngine.Random.Range(-100, 100);
        float startingZForce = UnityEngine.Random.Range(-100, 100);

        if (startingXForce * startingZForce < 50)
        {
            startingXForce = 100;
        }

        rb.AddForce(new Vector3(startingXForce, 0, startingZForce));
    }

    // Update is called once per frame
    void Update () {
		//if(Input.GetKeyDown("space"))
        //{
        //    LaunchBall();
        //}
	}

    private void FixedUpdate()
    {
        if(rb.velocity.magnitude < minimumVelocity)
        {
            rb.AddForce(rb.velocity.normalized * minimumVelocity);
        }
    }
}
