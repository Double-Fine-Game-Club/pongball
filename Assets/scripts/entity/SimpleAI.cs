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

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

        if (trackedBall)
        {
            if (TrackedBallIsOutsideBounds())
            {
                FollowTrackedBall();
            }
        }
        else
        {
            FindBall();
        }
    }

    private new void Update()
    {
        base.Update();
        if(myPowers.Count>0 || currentPowerName!="")
        {
            TryActivate();
        }

    } 

    private void FollowTrackedBall()
    {
		float dir = trackedBall.transform.position.z - transform.position.z;
        MovePaddles(dir);

        // TODO: Where do we put this
        // If tracked ball is within a certain distance, trigger pull animation
        var dist = Vector3.Distance(trackedBall.transform.position, transform.position);
        if (dist < 3 && dist > 1.5f)
        {
            animator.SetBool("pull", true);
            animator.SetBool("hit", true);
            SendInput("Fire1", true);
        }
        else
        {
            animator.SetBool("pull", false);
            animator.SetBool("hit", false);
            SendInput("Fire1", false);
        }
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

    new public void TryActivate()
    {
        if (!NetworkManager.singleton.isNetworkActive 
            || NetworkServer.connections.Count > 0
            && myPowers.Count>0 )
        {
            myPowers[myPowers.Count - 1].Activate();
        }
        else
        {
            //Client AI only needs to update the UI
            currentPowerName = "";
        }
        powerText.text = "";
    }

}
