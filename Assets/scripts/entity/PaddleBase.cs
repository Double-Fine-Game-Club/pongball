using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Base class for paddles
/// </summary>

public class PaddleBase : NetworkBehaviour {

    private float thrust = 10.0f;
    private float paddleLimitZ = 5.0f;

    private Vector3 up = new Vector3(1, 0, 0);

    private Rigidbody rigidBody;
    protected Animator animator;

    public int playerIndex;

    protected List<SuperPowerBase> myPowers = new List<SuperPowerBase>();
    public string currentPowerName = "";
    protected Text powerText;

    protected Dictionary<string, bool> remoteInputs = new Dictionary<string, bool>();
    protected float messageTimer = 0.3f;
    protected float timeToNextMessage;


    public virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();


        PaddleNetworking[] playerList = GameObject.FindObjectsOfType<PaddleNetworking>();
        int index = 0;
        foreach (PaddleNetworking player in playerList)
        {
            if (player.gameObject == gameObject)
            {
                playerIndex = index;
            }
            ++index;

        }

        currentPowerName = "";

        RaycastHit hit;
        LayerMask mask = 1 << 10;   //wall
        Vector3 offset = new Vector3(0, .5f, 0);//spawner is in the floor -_-
        if (Physics.Raycast(transform.position + offset, Vector3.forward, out hit, 10, mask))
        {
            float paddleColliderHalfSize = 1f;
            paddleColliderHalfSize = GetComponent<MeshCollider>().bounds.size.y/2; //collision precise
            paddleLimitZ = hit.point.z - paddleColliderHalfSize;
        }
        
        
    }

    private void OnEnable()
    {
    }


    protected void SetThrust(float thrust)
	{
		this.thrust = thrust;
	}

	// +ve for "up"
    protected void MovePaddles(float dir)
    {
		Debug.Assert(Time.inFixedTimeStep, "Paddle movement should only happen inside physics code");

		if (rigidBody)
        {

            //Stop momentum caused by switching controllers
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;

            // Invert for one side
            if (transform.position.x < 0) dir *= -1;

            // Clamp between [-1,1]
            dir = Mathf.Clamp(dir, -1.0f, 1.0f);

			//rigidBody.AddForce(dir * up * Thrust);
			transform.Translate(dir * up * thrust * Time.fixedDeltaTime);
		}
    }

    internal void FixedUpdate()
    {
        
        // Clamp Z if we're outside an arbitrary value
        //  Assumed symettric table
        if(transform.position.z < -paddleLimitZ || transform.position.z > paddleLimitZ)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                Mathf.Clamp(transform.position.z, -paddleLimitZ, paddleLimitZ)
                );
        }
    }

    internal void Update()
    {
        //Try to cleanup oldest power
        if (myPowers.Count > 0)
        {
            SuperPowerBase p = myPowers[0];
            if(!p.isActive && !p.isReady)
            {
                myPowers.RemoveAt(0);
                Destroy(p);
                //Debug.Log("Cleaning");
            }
        }
        
    }

    internal void TryActivate()
    {
     
    }

    public void AddPower(string powerName)
    {
        if (myPowers.Count > 0)
            { myPowers[myPowers.Count - 1].isReady = false; }
        //Debug.Log(powerName);
        SuperPowerBase spb = gameObject.AddComponent(Type.GetType(powerName)) as SuperPowerBase;
        spb.isReady = true;
        myPowers.Add(spb);

        if (!powerText)
        {
            GameObject powerUI = GameObject.FindGameObjectWithTag("PowerUp");
            powerText = powerUI.transform.GetChild(playerIndex).GetComponent<Text>();
        }
        powerText.text = powerName;
    }

    public void SetPower(string powerName)
    {
        //Debug.Log("Set Power: " + powerName);
        currentPowerName = powerName;

        if (!powerText)
        {
            GameObject powerUI = GameObject.FindGameObjectWithTag("PowerUp");
            powerText = powerUI.transform.GetChild(playerIndex).GetComponent<Text>();
        }
        powerText.text = powerName;
    }

    public virtual void SendInput(string input, bool isPress)
    {
        if (NetworkManager.singleton.isNetworkActive)
        {
            bool willSend= false;
            if (!remoteInputs.ContainsKey(input))
            {
                remoteInputs.Add(input, isPress);
                willSend = true;
            }
            else
            {
                bool lastState = remoteInputs[input];   //needs optimizing
                willSend = (lastState != isPress);
            }
            
            if (!willSend) return;

            PaddleNetworking pn = GetComponent<PaddleNetworking>();
            if (pn.isServer)
            {
                pn.RpcSendInput(input, isPress);
            }
            else
            {
                //isClient
                pn.CmdSendInput(input, isPress);
            }
            remoteInputs[input] = isPress;
        }
    }

    public void RecieveRemoteInput(string input, bool isPressed)
    {
        //Only works because its animations
        if (input.Contains("Fire1"))   //ctrl+v'd from player
        {
            if (isPressed)
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
    }
}

