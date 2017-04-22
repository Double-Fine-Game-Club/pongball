using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerManager : MonoBehaviour {

    private const double nextPowerTimer = 10.0;

    private PaddleBase paddle;
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
        TRIBAR,
        LAZER,
        WALL,
        SLOW,
        STORM,
        POWER_COUNT
    };
    


	// Use this for initialization
	void Start () {
        paddle = GetComponent<PaddleBase>();
        timeSinceGivenPower = 0;

        ownerId = paddle.netId.Value;

        //Build dict
        powerMapping = new Dictionary<powerTypes, SuperPowerBase>();
        powerMapping[powerTypes.EMPTY] = new SuperPowerBase(paddle);
        powerMapping[powerTypes.OBSCURE] = new Obscure(paddle);
        powerMapping[powerTypes.OBSTRUCT] = new Obstruct(paddle);
        powerMapping[powerTypes.TRIBAR] = new TriBar(paddle);
        powerMapping[powerTypes.LAZER] = new Lazer(paddle);
        powerMapping[powerTypes.WALL] = new Wall(paddle);
        powerMapping[powerTypes.SLOW] = new Slow(paddle);
        powerMapping[powerTypes.STORM] = new Storm(paddle);

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

        if(Input.GetButton("Fire1"))
        {
            currentPower.Activate();
            //TODO
            //Tell network power has activated
        }

	}

    void giveNewPower()
    {
        oldPower = currentPower;
        float randFloat = Random.Range(1, (float)(powerTypes.POWER_COUNT-1));
        int randInt = Mathf.FloorToInt(randFloat);
        currentPower = powerMapping[(powerTypes)randInt];
        currentPower.Ready();

        Debug.Log("Power Granted: " + randInt.ToString() + " to player " + ownerId);
        //TODO
        //Tell network which power this player has
    }
}
