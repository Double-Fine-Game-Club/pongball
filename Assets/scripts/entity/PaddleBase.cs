using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Base class for paddles
/// </summary>

public class PaddleBase : NetworkBehaviour {
	
    private float thrust = 10.0f;
    private float paddleLimitZ = 5.0f;

    private Vector3 up = new Vector3(1, 0, 0);

    private Rigidbody rigidBody;
    protected Animator animator;

    protected List<SuperPowerBase> myPowers = new List<SuperPowerBase>();
    protected string currentPowerName="";

    protected Dictionary<string, bool> remoteInputs = new Dictionary<string, bool>();
    
    public virtual void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
	}

    private void OnEnable()
    {
        currentPowerName = "";
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
            Vector3 zero = new Vector3(0, 0, 0);
            rigidBody.velocity = zero;
            rigidBody.angularVelocity = zero;

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

    internal void TryActivate() {}

    public void AddPower(string powerName)
    {
        if (myPowers.Count > 0)
            { myPowers[myPowers.Count - 1].isReady = false; }
        Debug.Log(powerName);
        SuperPowerBase spb = gameObject.AddComponent(Type.GetType(powerName)) as SuperPowerBase;
        spb.isReady = true;
        myPowers.Add(spb);
    }

    public void SetPower(string powerName)
    {
        Debug.Log("Set Power: " + powerName);
        currentPowerName = powerName;
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

