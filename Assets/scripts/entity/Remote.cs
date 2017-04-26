using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
        if (!powerText)
        {
            GameObject powerUI = GameObject.FindGameObjectWithTag("PowerUp");
            powerText = powerUI.transform.GetChild(playerIndex).GetComponent<Text>();
        }
        powerText.text = "";
    }
}
