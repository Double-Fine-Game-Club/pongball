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

    enum powerTypes {
        EMPTY=0,
        OBSCURE=1,
        OBSTRUCT=2,
        TRIBAR=3,
        LAZER=4,
        WALL=5,
        POWER_COUNT
    };
    


	// Use this for initialization
	void Start () {
        paddle = GetComponent<PaddleBase>();
        timeSinceGivenPower = 0;

        //Build dict
        powerMapping = new Dictionary<powerTypes, SuperPowerBase>();
        powerMapping[powerTypes.EMPTY] = new SuperPowerBase();
        powerMapping[powerTypes.OBSCURE] = new Obscure();
        powerMapping[powerTypes.OBSTRUCT] = new Obstruct();
        powerMapping[powerTypes.TRIBAR] = new TriBar();
        powerMapping[powerTypes.LAZER] = new Lazer();
        powerMapping[powerTypes.WALL] = new Wall();

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

        //TODO
        //Tell network which power this player has
    }
}
