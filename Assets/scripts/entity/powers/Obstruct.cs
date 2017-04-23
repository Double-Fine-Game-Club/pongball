using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Obstruct : SuperPowerBase {

    public static string obstruction = "entities/powers/obstruction";
    GameObject obstacle;
   
	// Use this for initialization
	void Start () {
        duration = 8;
        powerName = "Obstruct";
    }

    private void OnEnable()
    {
        duration = 8;
        powerName = "Obstruct";
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }


    override protected void TriggerEffect()
    {
        if (!isHost) { return; }
        try
        {
            obstacle = Instantiate(NetworkManager.singleton.spawnPrefabs[4]);
            //Get opponents paddle spawn position and block that
            PaddleBase[] paddles = Object.FindObjectsOfType<PaddleBase>();
            foreach(PaddleBase p in paddles)
            {
                if(p.gameObject != this.gameObject)
                {
                    obstacle.transform.position = p.transform.position;
                    break;
                }
            }
            NetworkServer.Spawn(obstacle);
        }
        catch
        {
            Debug.Log("Loading of \"" + obstruction + "\" failed");
        }
    }

    override protected void CleanUp()
    {
        if (isHost)
        {
            Object.Destroy(obstacle);
        }
        
        base.CleanUp();
    }
}
