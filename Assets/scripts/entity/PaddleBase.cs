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

    private Vector3 up = new Vector3(1, 0, 0);

    private Rigidbody rigidBody;
    protected Animator animator;

    protected List<SuperPowerBase> myPowers = new List<SuperPowerBase>();
    protected string currentPowerName="";
    
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
        if(transform.position.z < 4.0f || transform.position.z > 4.0f)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                Mathf.Clamp(transform.position.z, -4.0f, 4.0f)
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
                Debug.Log("Cleaning");
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
}
