using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SuperPowerBase {

    protected string powerName;
    protected int id;
    protected float duration;
    protected float remainingDuration;
    protected bool isActive;
    protected bool isReady;
    protected PaddleBase paddle;

    public SuperPowerBase(PaddleBase owner)
    {
        paddle = owner;
    }

	// Use this for initialization
	void Start () {
        isReady = false;
        isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            remainingDuration -= Time.deltaTime;
            if(remainingDuration <0)
            {
                CleanUp();
            }
        }
	}

    public void Ready()
    {
        isReady = true;

    }

    public void Activate() {
        
        if (isReady)
        {
            //Reset the duration if it is already active
            Debug.Log("Activated");
            isActive = true;
            isReady = false;
            remainingDuration = duration;
        }
        

    }

    private void CleanUp() {
        isActive = false;
    }

}
