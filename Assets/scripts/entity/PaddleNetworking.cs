using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

/// <summary>
/// Tracks which client is controlling the paddle
/// </summary>
public class PaddleNetworking : NetworkBehaviour {

    // Message handle for the client id message
    private short ID_MESSAGE = 1002;

	// The id for an AI controlled paddle (-1 may be Local)
	public static int PADDLE_AI = -2;

	// The id of the client controllign the paddle, server side replicated on clients
	[SyncVar]
	public int paddleClientId;

	// The index of the paddle on the table, server side replicated on clients
	[SyncVar]
	public int paddleIndex;

	new void OnStartClient () 
	{
		// The paddle is by default AI controlled
		paddleClientId = PADDLE_AI;
	}

	void Start()
	{
		// The paddle is default AI, register the message callback
		paddleClientId = PADDLE_AI;
		NetworkServer.RegisterHandler(ID_MESSAGE, OnReceiveClientId);
    }

	// Is the paddle being controlled by an AI
	public bool IsAIControlled()
	{
		return paddleClientId == PADDLE_AI;
	}

	// Returns the id of the client controlling the paddle
	public int GetClientId()
	{
		return paddleClientId;
	}

	// Updates the id of the client for this paddle on the server
	public void PossessPaddle()
	{
		// If this client is not already controlling the paddle
		if (paddleClientId != NetworkManager.singleton.client.connection.connectionId)
		{
			// Send a message to the server with the client id
			SendClientId(NetworkManager.singleton.client.connection.connectionId);
		}
	}

	// Updates the this paddle to be AI controlled on the server
	public void UnPossessPaddle()
	{
		// If this client controls the paddle
		if (paddleClientId == NetworkManager.singleton.client.connection.connectionId)
		{
			// Send a message to the server with the AI 
			SendClientId(PADDLE_AI);
		}
	}

	// Set the index of this paddle on the table
	public void SetPaddleIndex(int index)
	{
		paddleIndex = index;
	}

	// Get the index of this paddle on the table
	public int GetPaddleIndex()
	{
		return paddleIndex;
	}

	// Send network message to set the client id on the server
	void SendClientId(int clientId)
	{
		// The message contains the client id and the paddle index
		StringMessage msg = new StringMessage(clientId.ToString() + "." + paddleIndex.ToString());
		NetworkManager.singleton.client.connection.Send(ID_MESSAGE, msg);
	}

	// Callback for when a message is received
	void OnReceiveClientId(NetworkMessage netMsg)
	{
		// Only handle the message as the server
		if (NetworkServer.connections.Count > 0)
		{
			// Parse the message into the client id and paddle index
			string msg = netMsg.ReadMessage<StringMessage>().value; 
			string[] parts = msg.Split('.');
			int clientId = int.Parse(parts[0]);
			int index = int.Parse(parts[1]);

			// Find the paddle of the given index
			foreach (PaddleNetworking paddle in GameObject.FindObjectsOfType<PaddleNetworking>())
			{
				if (paddle.GetPaddleIndex() == index)
				{
					// Set the id to the given client id
					paddle.SetClientId(clientId);
				}
			}
		}
	}

	// Set the client id controlling this paddle
	public void SetClientId(int clientId)
	{
		// If the paddle is being controlled by AI (does not override other client possession)
		if (paddleClientId == PADDLE_AI && clientId != PADDLE_AI)
		{
			// Disable the AI for this paddle on the server
			GetComponent<SimpleAI>().enabled = false;
            GetComponent<Remote>().enabled = true;

			// Find the client which is taking possession
			foreach (NetworkConnection conn in NetworkServer.connections)
			{
				if (conn.connectionId == clientId)
				{
					// Assign that client control over the paddle
					GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
					break;
				}
			}
		}
		// If the paddle is being controlled by a client
		else if (paddleClientId != PADDLE_AI && clientId == PADDLE_AI)
		{
			// Find the client which has possession
			foreach (NetworkConnection conn in NetworkServer.connections)
			{
				if (conn.connectionId == paddleClientId)
				{
					// Remove the client's control over the paddle
					GetComponent<NetworkIdentity>().RemoveClientAuthority(conn);
					break;
				}
			}

			// Enabled the AI for this paddle on the server
			GetComponent<SimpleAI>().enabled = true;
            GetComponent<Remote>().enabled = false;
		}

		// Update the client id on the server (is replicated on clients)
		paddleClientId = clientId;
	}

    /****
     * powers
     *****///
    [ClientRpc]
    public void RpcSetCurrentPower(string newPower)
    {
        PaddleBase[] pb = gameObject.GetComponents<PaddleBase>();
        foreach (PaddleBase p in pb)
        {
            if (p.enabled)
            {
                p.SetPower(newPower);
            }

        }
    }

    [Command]
    public void CmdActivatePower()
    {
        //Only remote players can send this command
        //Debug.Log("Message Recieve: Use Power");
        Remote r = gameObject.GetComponent<Remote>();

        if(r.enabled)
        {
            r.TryActivate();
        }

    }

    /***
     * player inputs
     * ****/
     [Command]
     public void CmdSendInput(string input, bool isPressed)
    {
        //Only remote players can send this command
        //Debug.Log("Message Recieve: Send Input");
        Remote r = gameObject.GetComponent<Remote>();
        if(r.enabled)
        {
            r.RecieveRemoteInput(input, isPressed);
        }
    }

    [ClientRpc]
    public void RpcSendInput(string input, bool isPressed)
    {
        PaddleBase[] pb = gameObject.GetComponents<PaddleBase>();
        foreach(PaddleBase p in pb)
        {
            if(p.enabled)
            {
                p.RecieveRemoteInput(input, isPressed);
            }
        }
    }



    
}
