using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class for AI controlled paddles
/// </summary>

public class SimpleAI : PaddleBase {

    private Transform trackedBall = null;

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
            trackedBall = ball.transform;
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        if(trackedBall)
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
		float dir = trackedBall.transform.position.z - transform.position.z;
		MovePaddles(-dir);
    }

    private bool TrackedBallIsOutsideBounds()
    {
        if(GetComponent<Collider>())
        {
            float topBounds = transform.position.z - GetComponent<Collider>().bounds.size.z / 2;
            float bottomBounds = transform.position.z + GetComponent<Collider>().bounds.size.z / 2;

            return  trackedBall.position.z < topBounds ||
                    trackedBall.position.z > bottomBounds;
        }
        return trackedBall.position.z < transform.position.z;
    }
}
