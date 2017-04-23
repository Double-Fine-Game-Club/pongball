using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
        if (!isHost) { return; }

        try
        {
            zone = Instantiate(NetworkManager.singleton.spawnPrefabs[3]);
            zone.transform.position = gameObject.transform.position;
            NetworkServer.Spawn(zone);
        }
        catch
        {
            Debug.Log("Loading of \"" + slow_zone + "\" failed");
        }
    }

     override protected void CleanUp()
    {
        if (isHost)
        {
            Object.Destroy(zone);
        }
        
        base.CleanUp();
    }
}
