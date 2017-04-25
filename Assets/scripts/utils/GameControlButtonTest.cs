using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlButtonTest : MonoBehaviour {
    //This doesn't work very well.
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.anyKey)
        {
            Debug.Log(Input.inputString);
        }
      
	}
}
