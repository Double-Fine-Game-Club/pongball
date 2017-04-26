using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class SuperPowerBase : NetworkBehaviour{

    protected string powerName;
    protected int id;
    protected double duration;
    protected double remainingDuration;
    public bool isActive { get; protected set; }
    public bool isReady { get; set; }
    public bool isHost { get; protected set; }

    SFXPlayer mySounds;

    // Use this for initialization
    void Start () {
        mySounds = gameObject.GetComponent<SFXPlayer>();
        SoundManager.instance.PlaySingle(mySounds.specialReady);
	}


	
	// Update is called once per frame
	virtual public void Update () {
        
        if (isActive)
        {
            remainingDuration -= Time.deltaTime;
            if (remainingDuration <0)
            {
                mySounds = gameObject.GetComponent<SFXPlayer>();
                SoundManager.instance.PlaySingle(mySounds.specialEnd);
                CleanUp();
            }
        }
	}

    public void Activate() {
        
        if (isReady)
        {
            isHost = !NetworkManager.singleton.isNetworkActive || NetworkServer.connections.Count > 0;

            //Reset the duration if it is already active
            remainingDuration = duration;
            TriggerEffect();
            isActive = true;
            isReady = false;
            mySounds = gameObject.GetComponent<SFXPlayer>();
            SoundManager.instance.PlaySingle(mySounds.specialActivate);
        }
        

    }

    virtual protected void TriggerEffect()
    {

    }

    virtual protected void CleanUp() {
        isActive = false;
        isReady = false;
    }

}
