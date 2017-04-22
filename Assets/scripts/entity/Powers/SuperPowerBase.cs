using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SuperPowerBase {

    protected string powerName;
    protected int id;
    protected double duration;
    protected double remainingDuration;
    protected bool isActive;
    protected bool isReady;
    protected GameObject paddle;

    public SuperPowerBase(GameObject owner)
    {
        paddle = owner;
    }

	// Use this for initialization
	void Start () {
        isReady = false;
        isActive = false;
        remainingDuration = 0;
	}
	
	// Update is called once per frame
	virtual public void Update () {
        
        if (isActive)
        {
            remainingDuration -= Time.deltaTime;
            if (remainingDuration <0)
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
            remainingDuration = duration;
            TriggerEffect();
            isActive = true;
            isReady = false;
            
        }
        

    }

    virtual protected void TriggerEffect()
    {

    }

    virtual protected void CleanUp() {
        isActive = false;
    }

}
