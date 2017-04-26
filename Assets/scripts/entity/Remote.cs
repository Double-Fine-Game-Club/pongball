using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Remote : PaddleBase {

	// Update is called once per frame
	new void Update () {
        base.Update();
	}

    new public void TryActivate()
    {
        if (!NetworkManager.singleton.isNetworkActive 
            || NetworkServer.connections.Count > 0
            && myPowers.Count > 0)
        {
            myPowers[myPowers.Count - 1].Activate();
        }
        powerText.text = "";
    }
}
