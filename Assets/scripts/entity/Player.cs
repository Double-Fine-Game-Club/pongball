using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Player controlled paddle
/// </summary>

public class Player : PaddleBase {
    
    public int playerNum = 1;
    [Range(0.4f, 1.3f)]
    public float paddleSpeed;

    // Use this for initialization
    public override void Start()
    {
        //playerNum = 1;
		base.Start();
        paddleSpeed = .85f;
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
            MovePaddles(vaxis * paddleSpeed);
            //Debug.Log(vaxis * paddleSpeed);
        }

        // If Fire1 is pressed, trigger pull animation
        //if (Input.GetButton("Fire1"))
        if (Input.GetButton(playerNum+"Fire1"))

        {
            animator.SetBool("pull", true);
            animator.SetBool("hit", true);
            SendInput(playerNum + "Fire1", true);
        }
        else
        {
            animator.SetBool("pull", false);
            animator.SetBool("hit", false);
            SendInput(playerNum + "Fire1", false);
        }
    }

    private new void Update()
    {
        base.Update();
        if (Input.GetButton(playerNum+"Fire2") && ( myPowers.Count > 0 || currentPowerName!=""))
        {
            TryActivate();
        }
        timeToNextMessage -= Time.deltaTime;

    }

    new public void TryActivate()
    {
        if (timeToNextMessage < 0)
        {
            timeToNextMessage = messageTimer;

            PaddleNetworking pn = GetComponent<PaddleNetworking>();
            if (!NetworkManager.singleton.isNetworkActive || NetworkServer.connections.Count > 0)
            {
                myPowers[myPowers.Count - 1].Activate();
                currentPowerName = "";
            }
            else
            {
                pn.CmdActivatePower();
                currentPowerName = "";
            }

            if (pn.isServer)
            {
                pn.RpcSetCurrentPower("");
            }

            if (!powerText)
            {
                GameObject powerUI = GameObject.FindGameObjectWithTag("PowerUp");
                powerText = powerUI.transform.GetChild(playerIndex).GetComponent<Text>();
            }

            powerText.text = "";
        }
    }
}
