using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : SuperPowerBase {

    public static string slow_zone = "entities/slow_zone";
    GameObject zone;

    public Slow(PaddleBase owner) : base(owner)
    {
        duration = 5.0;
        powerName = "Slow";
    }
	// Use this for initialization
	void Start () {
       

	}

    // Update is called once per frame
    override public void Update () {
        base.Update();
	}


     override protected void TriggerEffect()
    {
        try
        {
            zone = Object.Instantiate(Resources.Load(slow_zone)) as GameObject;
            zone.transform.position = paddle.transform.position;
            
        }
        catch
        {
            Debug.Log("Loading of \"" + slow_zone + "\" failed");
        }
    }

     override protected void CleanUp()
    {
        Object.Destroy(zone);
        base.CleanUp();
    }
}
