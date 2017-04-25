using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Obscure : SuperPowerBase {

    public static string obscuration = "entities/powers/obscuration";
    GameObject obstacle;
   // Use this for initialization

	void Start () {
        duration = 8;
        powerName = "Obscure";
	}

    private void OnEnable()
    {
        duration = 8;
        powerName = "Obscure";
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
            obstacle = Instantiate(NetworkManager.singleton.spawnPrefabs[10]);
             //Get opponents paddle spawn position and block that
            PaddleBase[] paddles = Object.FindObjectsOfType<PaddleBase>();
            foreach (PaddleBase p in paddles)
            {
                if (p.gameObject != this.gameObject)
                {
                    obstacle.transform.position = p.transform.position + new Vector3(0,3,0);
                    break;
                }
            }
            if (NetworkServer.active)
            {
                NetworkServer.Spawn(obstacle);
            }
                
        }
        catch
        {
            Debug.Log("Loading of \"" + obscuration + "\" failed");
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
