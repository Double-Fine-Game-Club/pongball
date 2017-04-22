using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruct : SuperPowerBase {

    public static string obstruction = "entities/powers/obstruction";
    GameObject obstacle;
    public Obstruct(GameObject owner) : base(owner)
    {
        duration = 5;
        powerName = "Obstruct";
    }
	// Use this for initialization
	void Start () {	}

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }


    override protected void TriggerEffect()
    {
        try
        {
            obstacle = Object.Instantiate(Resources.Load(obstruction)) as GameObject;
            //Get opponents paddle spawn position and block that
            PaddleBase[] paddles = Object.FindObjectsOfType<PaddleBase>();
            foreach(PaddleBase p in paddles)
            {
                if(p.gameObject != paddle)
                {
                    obstacle.transform.position = p.transform.position;
                    break;
                }
            }
        }
        catch
        {
            Debug.Log("Loading of \"" + obstruction + "\" failed");
        }
    }

    override protected void CleanUp()
    {
        Object.Destroy(obstacle);
        base.CleanUp();
    }
}
