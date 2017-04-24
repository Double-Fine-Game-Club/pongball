using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PowerManager : NetworkBehaviour {

    private const double nextPowerTimer = 30.0;

    private GameObject paddle;
    private double timeSinceGivenPower;
    private Dictionary<powerTypes, string> powerMapping;
    private bool isHost;
    private PaddleBase[] playerList;

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

        //Host of online game or local game
        isHost = !NetworkManager.singleton.isNetworkActive || NetworkServer.connections.Count > 0;

        //Build dict
        powerMapping = new Dictionary<powerTypes, string>();
        powerMapping[powerTypes.EMPTY] = "";
        powerMapping[powerTypes.OBSCURE] = "Obscure";
        powerMapping[powerTypes.OBSTRUCT] = "Obstruct";
     //   powerMapping[powerTypes.TRIBAR] = //new TriBar(paddle);
     //   powerMapping[powerTypes.LAZER] = //new Lazer(paddle);
     //   powerMapping[powerTypes.WALL] = //new Wall(paddle);
        powerMapping[powerTypes.SLOW] = "Slow";
     //   powerMapping[powerTypes.STORM] = //new Storm(paddle);

        playerList = GameObject.FindObjectsOfType<PaddleBase>();
       //Note: each paddle can have multiple scripts attached

    }

    private void Awake()
    {
        timeSinceGivenPower = 0;
    }

    // Update is called once per frame
    void Update () {
        if (isHost)
        {

            timeSinceGivenPower += Time.deltaTime;
            if (nextPowerTimer - timeSinceGivenPower < 0)
            {
                timeSinceGivenPower -= nextPowerTimer;

                //all powers are given at the same time
                foreach (PaddleBase player in playerList)
                {
                    
                    //players can be swapped with AI by toggling scripts
                    if (player.enabled)
                    {
                        giveNewPower(player);
                    
                    }
                    

                }


            }
        }

	}

    
    void giveNewPower(PaddleBase player)
    {
         float randFloat = Random.Range(1, (float)(powerTypes.POWER_COUNT));
        int randInt = Mathf.FloorToInt(randFloat);
        string powerName = powerMapping[(powerTypes)randInt];
        
        player.AddPower(powerName);

        Debug.Log("Power Granted: " + powerMapping[(powerTypes)randInt].ToString() + " to player " + ownerId);

        if (NetworkManager.singleton.isNetworkActive)
        {
            PaddleNetworking pNet = player.gameObject.GetComponent<PaddleNetworking>();
            pNet.RpcSetCurrentPower(powerName);
        }
    }


    //Util

    //Comparator is the gameObject field
    public PaddleBase[] MakeUnique(PaddleBase[] input)
    {
        List<PaddleBase> toKeep = new List<PaddleBase>();
        foreach (PaddleBase pb in input)
        {
            bool keep = true;
            foreach (PaddleBase p in toKeep)
            {
                if (p.gameObject == pb.gameObject)
                {
                    keep = false;
                    break;
                }

            }
            if (keep)
            {
                toKeep.Add(pb);
            }
        }
        return toKeep.ToArray();
    }
}
