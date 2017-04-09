using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class for AI controlled paddles
/// </summary>

public class SimpleAI : PaddleBase {

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
		float dir = TrackedBall.transform.position.x - transform.position.x;
		MovePaddles(dir);
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
