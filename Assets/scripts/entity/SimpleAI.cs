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
		float dir = trackedBall.transform.position.x - transform.position.x;
		MovePaddles(dir);
    }

    private bool TrackedBallIsOutsideBounds()
    {
        if(GetComponent<Collider>())
        {
            float topBounds = transform.position.x - GetComponent<Collider>().bounds.size.x / 2;
            float bottomBounds = transform.position.x + GetComponent<Collider>().bounds.size.x / 2;

            return  trackedBall.position.x < topBounds ||
                    trackedBall.position.x > bottomBounds;
        }
        return trackedBall.position.x < transform.position.x;
    }
}
