using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ResetBallUI : NetworkBehaviour {

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 150, 10, 140, 40));
        if (GUILayout.Button("Reset Ball Position"))
        {
            CmdResetBallPosition();
        }
        GUILayout.EndArea();
    }

    [Command]
    private void CmdResetBallPosition()
    {
        var ball = GameObject.FindGameObjectWithTag("Ball");
        if(ball != null)
        {
            ball.GetComponent<Ball>().ResetPosition();
        }
    }
}
