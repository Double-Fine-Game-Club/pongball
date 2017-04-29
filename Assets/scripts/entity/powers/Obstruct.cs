using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Obstruct : SuperPowerBase {

    public static string obstruction = "entities/powers/obstruction";
    GameObject obstacle;
    List<PaddleBase> targets = new List<PaddleBase>();
   
	// Use this for initialization
	void Start () {
        duration = 5;
        powerName = "Obstruct";
    }

    private void OnEnable()
    {
        duration = 5;
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
            obstacle = Instantiate(NetworkManager.singleton.spawnPrefabs[9]);
            //Get opponents paddle spawn position and block that
            PaddleBase[] paddles = Object.FindObjectsOfType<PaddleBase>();
            foreach(PaddleBase p in paddles)
            {
                if(p.gameObject != this.gameObject)
                {
                    float direction = FindClosestWall(p.transform.position);
                    obstacle.transform.position = p.transform.position + new Vector3(0, 0, direction) ;
                    p.Obstruct(obstacle.transform.position.z);
                    targets.Add(p);
                }
            }
            if (NetworkServer.active)
            {
                NetworkServer.Spawn(obstacle);
                ObstructClient();
            }
                
        }
        catch
        {
            Debug.Log("Loading of \"" + obstruction + "\" failed");
        }
    }

    override protected void CleanUp()
    {
        foreach(PaddleBase p in targets)
        {
            p.Obstruct(0, true);
        }
        if (isHost)
        {
            ObstructClient(true);
            Object.Destroy(obstacle);
        }
        
        base.CleanUp();
    }

    //return +1 for above or -1 for below
    private float FindClosestWall(Vector3 point)
    {
        point += new Vector3(0, 1, 0);  //To make the flippers table work
        LayerMask mask = 1 << 10;   //wall
        RaycastHit hit;
        float hit1, hit2;
        Physics.Raycast(point, new Vector3(0, 0, 1), out hit, 10, mask);
        hit1 = Mathf.Abs(hit.distance);
        Physics.Raycast(point, new Vector3(0, 0, -1), out hit, 10, mask);
        hit2 = Mathf.Abs(hit.distance);
        Debug.Log(hit1);
        Debug.Log(hit2);
        float direction = 1;
        direction = hit1 < hit2 ? direction : -direction;
        Debug.Log(direction);
        return direction;
    }

    protected void ObstructClient(bool isCleanup=false)
    {
        if (!isHost || !NetworkManager.singleton.isNetworkActive) return;

        PaddleNetworking pn = targets[0].GetComponent<PaddleNetworking>();
        pn.RpcObstructMe(obstacle.transform.position.z, isCleanup);

    }

}
