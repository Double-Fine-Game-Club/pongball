using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slow_zone_collision : MonoBehaviour {

    List<GameObject> effectedObjects = new List<GameObject>();
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag=="Ball")
        {
            //Slow the ball
            effectedObjects.Add(other.gameObject);
            other.gameObject.GetComponent<Ball>().ClampSpeed(0, 1);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            //Un-slow the ball
            other.gameObject.GetComponent<Ball>().UnClampSpeed();
            effectedObjects.Remove(other.gameObject);
        }
    }

    private void OnDestroy()
    {
        foreach(GameObject obj in effectedObjects)
        {
            //undo any existing effects
            obj.GetComponent<Ball>().UnClampSpeed();
        }
    }
}
