using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ResetBallUI : NetworkBehaviour {

	// Message handle for the client id message
	private short RESPAWN_MESSAGE = 1003;

	void OnServerStart()
	{
		NetworkServer.RegisterHandler(RESPAWN_MESSAGE, OnResetBallPosition);
	}

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 150, 10, 140, 40));
        if (GUILayout.Button("Reset Ball Position"))
		{
			ResetBallPosition();
        }
        GUILayout.EndArea();
    }

    [Server]
	private void OnResetBallPosition(NetworkMessage netMsg)
    {
		ResetBallPosition();
    }

	private void ResetBallPosition()
	{
		if (NetworkServer.connections.Count > 0 || !NetworkManager.singleton.isNetworkActive)
		{
			// If local or the server reset the ball position
			var ball = GameObject.FindGameObjectWithTag("Ball");
			if(ball != null)
			{
				ball.GetComponent<Ball>().ResetPosition();
			}
		}
		else
		{
			// Send an empty message of type respawn message
			NetworkManager.singleton.client.connection.Send(RESPAWN_MESSAGE, new EmptyMessage());
		}
	}
}
