using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Player controlled paddle
/// </summary>

public class Player : PaddleBase {
    //use this in inspector on your prefab to assign player?  Will that work?  Idfk.  *crosses fingers* -sjm
    public int playerNum = 1;

	// Use this for initialization
	public override void Start()
    {
        playerNum = 1;
		base.Start();
		
		// Give the player faster movement
		SetThrust(20);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        //float haxis = Input.GetAxis("Horizontal");
        //float vaxis = Input.GetAxis("Vertical");

        //testing 2 player local
        float haxis = Input.GetAxis(playerNum+"Horizontal");
        float vaxis = Input.GetAxis(playerNum+"Vertical");
        //float vaxis = Input.GetAxis("1Vertical");


        //Local Multiplayer

        if (haxis != 0)
        {
            
        }

        if (vaxis != 0)
        {
            MovePaddles(vaxis);
        }

        // If Fire1 is pressed, trigger pull animation
        //if (Input.GetButton("Fire1"))
        if (Input.GetButton(playerNum+"Fire1"))

        {
            animator.SetBool("pull", true);
            animator.SetBool("hit", true);
        }
        else
        {
            animator.SetBool("pull", false);
            animator.SetBool("hit", false);
        }
    }

    private new void Update()
    {
        base.Update();
        //if (Input.GetKeyDown(KeyCode.Space) &&  (myPowers.Count > 0 || currentPowerName!=""))
        if (Input.GetButton(playerNum+"Fire2") &&  (myPowers.Count > 0 || currentPowerName!=""))
        {
            TryActivate();
        }
    }

    new public void TryActivate()
    {
        if (!NetworkManager.singleton.isNetworkActive || NetworkServer.connections.Count > 0)
        {
            myPowers[myPowers.Count - 1].Activate();
        }
        else
        {
            PaddleNetworking pn = gameObject.GetComponent<PaddleNetworking>();
            pn.CmdActivatePower();
            currentPowerName = "";
        }
    }
}
