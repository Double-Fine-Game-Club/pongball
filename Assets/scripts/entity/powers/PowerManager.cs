using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PowerManager : MonoBehaviour {

    private const double nextPowerTimer = 10.0;

    private GameObject paddle;
    private double timeSinceGivenPower;
    private SuperPowerBase currentPower;
    private SuperPowerBase oldPower;
    private Dictionary<powerTypes, SuperPowerBase> powerMapping;

    //Debugging
    private uint ownerId;

    enum powerTypes {
        EMPTY,
        OBSCURE,
        OBSTRUCT,
     //   TRIBAR,
     //   LAZER,
     //   WALL,
        SLOW,
     //   STORM,
        POWER_COUNT
    };
    


	// Use this for initialization
	void Start () {
        paddle = gameObject;
        timeSinceGivenPower = 0;

        
        //Build dict
        powerMapping = new Dictionary<powerTypes, SuperPowerBase>();
        powerMapping[powerTypes.EMPTY] = new SuperPowerBase(paddle);
        powerMapping[powerTypes.OBSCURE] = new Obscure(paddle);
        powerMapping[powerTypes.OBSTRUCT] = new Obstruct(paddle);
     //   powerMapping[powerTypes.TRIBAR] = new TriBar(paddle);
     //   powerMapping[powerTypes.LAZER] = new Lazer(paddle);
     //   powerMapping[powerTypes.WALL] = new Wall(paddle);
        powerMapping[powerTypes.SLOW] = new Slow(paddle);
     //   powerMapping[powerTypes.STORM] = new Storm(paddle);

        currentPower = powerMapping[powerTypes.EMPTY];
        oldPower = currentPower;

    }

    private void Awake()
    {
        timeSinceGivenPower = 0;
    }

    // Update is called once per frame
    void Update () {
        timeSinceGivenPower += Time.deltaTime;
        if(nextPowerTimer - timeSinceGivenPower < 0)
        {
            giveNewPower();
            timeSinceGivenPower -= nextPowerTimer;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            currentPower.Activate();

            //Tell network power has activated
			if (NetworkManager.singleton.isNetworkActive)
			{
				GetComponent<PaddleNetworking>().CmdUsePower();
			}
        }
        if (oldPower!=null) oldPower.Update();
        if (currentPower!=null) currentPower.Update();

	}

	void giveNewPower()
    {
		oldPower = currentPower;

        float randFloat = Random.Range(1, (float)(powerTypes.POWER_COUNT));
        int randInt = Mathf.FloorToInt(randFloat);
        currentPower = powerMapping[(powerTypes)randInt];
        currentPower.Ready();

        Debug.Log("Power Granted: " + powerMapping[(powerTypes)randInt].ToString() + " to player " + ownerId);
        
        //Tell network which power this player has
		if (NetworkManager.singleton.isNetworkActive)
		{
			GetComponent<PaddleNetworking>().CmdSetPower(randInt);
		}
    }

	// Called by the server to assign a non-player with a power
	public void AssignPower(int power)
	{
		oldPower = currentPower;

		currentPower = powerMapping[(powerTypes)power];
		currentPower.Ready();
	}

	// Called by the server to activate a power
	public void ActivatePower()
	{
		currentPower.Activate();
	}
}
