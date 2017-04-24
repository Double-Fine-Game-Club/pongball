using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ObstacleManager : NetworkBehaviour 
{
	private short ACTIVATE_MESSAGE = 1100;
	private short DEACTIVATE_MESSAGE = 1101;

	public ObstacleNetworking[] obstacles;

	[ServerCallback]
	void Start () 
	{
		// Register the message handlers on the server
		NetworkServer.RegisterHandler(ACTIVATE_MESSAGE, OnReceiveActivateMessage);
		NetworkServer.RegisterHandler(DEACTIVATE_MESSAGE, OnReceiveDeactivateMessage);
	}

	void Update () 
	{
		// Fill the obstacle list once the table is spawned
		if (obstacles.Length == 0 && Object.FindObjectOfType<ObstacleNetworking>() != null)
		{
			obstacles = Object.FindObjectsOfType<ObstacleNetworking>();
		}
	}

	// Activate the obstacle on the server
	public void ActivateObstacle(ObstacleNetworking obstacle)
	{
		IntegerMessage msg = new IntegerMessage(GetObstacleIndex(obstacle));
		NetworkManager.singleton.client.connection.Send(ACTIVATE_MESSAGE, msg);
	}

	// Deactivate the obstacle on the server
	public void DeactivateObstacle(ObstacleNetworking obstacle)
	{
		IntegerMessage msg = new IntegerMessage(GetObstacleIndex(obstacle));
		NetworkManager.singleton.client.connection.Send(DEACTIVATE_MESSAGE, msg);
	}

	// Callback for when an activate message is received
	[Server]
	void OnReceiveActivateMessage(NetworkMessage netMsg)
	{
		// Retrieve the obstacle index
		int obstacleIndex = netMsg.ReadMessage<IntegerMessage>().value;

		RpcActivateObstacle(obstacleIndex);
	}

	// Callback for when a deactivate message is received
	[Server]
	void OnReceiveDeactivateMessage(NetworkMessage netMsg)
	{
		// Retrieve the obstacle index
		int obstacleIndex = netMsg.ReadMessage<IntegerMessage>().value;

		RpcDeactivateObstacle(obstacleIndex);
	}

	// Activate the obstacle on all clients
	[ClientRpc]
	public void RpcActivateObstacle(int obstacleIndex)
	{
		ObstacleNetworking obstacle = obstacles[obstacleIndex];

		if (obstacle != null)
		{
			obstacle.ActivateFromServer();
		}
	}

	// Deactivate the obstacle on all clients
	[ClientRpc]
	public void RpcDeactivateObstacle(int obstacleIndex)
	{
		ObstacleNetworking obstacle = obstacles[obstacleIndex];

		if (obstacle != null)
		{
			obstacle.DeactivateFromServer();
		}
	}

	// Find the index of the obstacle
	public int GetObstacleIndex(ObstacleNetworking obstacle)
	{
		int obstacleIndex = 0;

		foreach (ObstacleNetworking o in obstacles)
		{
			if (obstacle.gameObject == o.gameObject)
			{
				return obstacleIndex;
			}

			obstacleIndex++;
		}

		return -1;
	}
}
