using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

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

        //This is kinda busted at the moment. It doesn't allow variants to be reset and doesn't clear event bindings. Commening out for now - Cheese
        /*if (GUILayout.Button("Exit to menu"))
        {
            ExitToMenu();
        }*/
        GUILayout.EndArea();
    }


    private void ExitToMenu()
    {
        // TODO: Proper way to exit?
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopHost();
        Destroy(NetworkManager.singleton.gameObject);
        NetworkServer.DisconnectAll();
        SceneManager.LoadScene("MainMenu");
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
