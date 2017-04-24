using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObstacleNetworking : MonoBehaviour 
{
	ObstacleManager obstacleManager;

	public delegate void ActivateCallback();
	public ActivateCallback ActivateFromServer;

	public delegate void DeactivateCallback();
	public ActivateCallback DeactivateFromServer;

	void Update () 
	{
		// Get the obstacle manager
		if (obstacleManager == null)
		{
			obstacleManager = Object.FindObjectOfType<ObstacleManager>();
		}
	}

	// Activate on the server and other clients
	public void ActivateOnServer()
	{
		if (NetworkManager.singleton.isNetworkActive)
		{
			obstacleManager.ActivateObstacle(this);
		}
	}

	// Deactivate on the server and other clients
	public void DeactivateOnServer()
	{
		if (NetworkManager.singleton.isNetworkActive)
		{
			obstacleManager.DeactivateObstacle(this);
		}
	}
}
