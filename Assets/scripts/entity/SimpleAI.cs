using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SimpleAI : NetworkBehaviour {

    private float Thrust = 20.0f;

    private Vector3 up = new Vector3(1, 0, 0);
    private Vector3 down = new Vector3(-1, 0, 0);

    private Transform TrackedBall = null;

    public override void OnStartLocalPlayer()
    {
        FindBall();
        if(isLocalPlayer)
        {
            GetComponent<MeshRenderer>().material.color = Color.cyan;
        }
    }

    private void FindBall()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball)
        {
            TrackedBall = ball.transform;
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        if(TrackedBall)
        {
            if (TrackedBallIsOutsideBounds())
            {
                FollowTrackedBall();
            }
        } else
        {
            FindBall();
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
        if(GetComponent<Rigidbody>())
        {
            var rb = GetComponent<Rigidbody>();

            GetComponent<Rigidbody>().velocity = up * Thrust;
        }
    }

    private void MoveDown()
    {
        if (GetComponent<Rigidbody>())
        {
            var rb = GetComponent<Rigidbody>();

            GetComponent<Rigidbody>().velocity = down * Thrust;
        }
    }

    private bool TrackedBallIsOutsideBounds()
    {
        if(GetComponent<Collider>())
        {
            float topBounds = transform.position.x - GetComponent<Collider>().bounds.size.x / 2;
            float bottomBounds = transform.position.x + GetComponent<Collider>().bounds.size.x / 2;

            return  TrackedBall.position.x < topBounds ||
                    TrackedBall.position.x > bottomBounds;
        }
        return TrackedBall.position.x < transform.position.x;
    }
}
