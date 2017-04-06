using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour {

    private float Thrust = 100.0f;

    private Vector3 up = new Vector3(1, 0, 0);
    private Vector3 down = new Vector3(-1, 0, 0);

    private Transform TrackedBall = null;
    private Rigidbody myRigidBody = null;
    private Collider myCollider = null;

    // Use this for initialization
    void Start () {
        GameObject ball = GameObject.Find("ball");
        myRigidBody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();

        if (ball)
        {
            TrackedBall = ball.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void FixedUpdate()
    {
        if(TrackedBall)
        {
            if (TrackedBallIsOutsideBounds())
            {
                FollowTrackedBall();
            }
        }
    }

    private void FollowTrackedBall()
    {
        if (TrackedBall.transform.position.x < transform.position.x)
            MoveDown();
        else
            MoveUp();
    }

    private void MoveUp()
    {
        if(myRigidBody)
        {
            myRigidBody.AddForce(up * Thrust);
        }
    }

    private void MoveDown()
    {
        if (myRigidBody)
        {
            myRigidBody.AddForce(down * Thrust);
        }
    }

    private bool TrackedBallIsOutsideBounds()
    {
        if(myCollider)
        {
            float topBounds = transform.position.x - myCollider.bounds.size.x / 2;
            float bottomBounds = transform.position.x + myCollider.bounds.size.x / 2;

            return  TrackedBall.position.x < topBounds ||
                    TrackedBall.position.x > bottomBounds;
        }
        return TrackedBall.position.x < transform.position.x;
    }
}
