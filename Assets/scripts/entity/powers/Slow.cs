using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : SuperPowerBase {

    public static string slow_zone = "entities/powers/slow_zone";
    GameObject zone;

	// Use this for initialization
	void Start () {
        duration = 8;
        powerName = "Slow";

    }
    private void OnEnable()
    {
        duration = 8;
        powerName = "Slow";
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
            zone.transform.position = gameObject.transform.position;
            
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
