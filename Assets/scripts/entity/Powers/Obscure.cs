using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obscure : SuperPowerBase {

    public static string obscuration = "entities/obscuration";
    GameObject obstacle;
    public Obscure(GameObject owner) : base(owner)
    {
        duration = 10;
    }
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }


    override protected void TriggerEffect()
    {
        try
        {
            obstacle = Object.Instantiate(Resources.Load(obscuration)) as GameObject;
            //Get opponents paddle spawn position and block that
            PaddleBase[] paddles = Object.FindObjectsOfType<PaddleBase>();
            foreach (PaddleBase p in paddles)
            {
                if (p.gameObject != paddle)
                {
                    obstacle.transform.position = p.transform.position + new Vector3(0,2,0);
                    break;
                }
            }
        }
        catch
        {
            Debug.Log("Loading of \"" + obscuration + "\" failed");
        }
    }

    override protected void CleanUp()
    {
        Object.Destroy(obstacle);
        base.CleanUp();
    }
}
