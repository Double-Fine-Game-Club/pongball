using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSetup : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	// Begin hosting using the given port
	public void BeginHosting(int port)
	{
		NetworkManager.singleton.networkPort = port;
		NetworkManager.singleton.StartHost();

		if (NetworkManager.singleton.isNetworkActive)
		{
			GameObject tableManager = GameObject.Instantiate(NetworkManager.singleton.spawnPrefabs[7]);
			NetworkServer.Spawn(tableManager);
		}
		else
		{
			Debug.Log("error starting host");
		}
	}

	// Connect to server using the given ip and port
	public bool ConnectToServer(string IP, int remotePort)
	{
		NetworkConnectionError error = Network.Connect(IP, remotePort);

		if (error == NetworkConnectionError.NoError)
		{
			return true;
		}
		else
		{
			Debug.Log("connection failed with error " + error.ToString());

			return false;
		}
	}

	// Attempts to set up the table on the client (selected by server) returning true if successful
	public bool LoadTableOnClient()
	{
		TableNetworking tableNetworking = null;
		GameObject g = GameObject.FindGameObjectWithTag("TableNetworking");
		if (g)
		{
			tableNetworking = g.GetComponent<TableNetworking>();
		}

		if (tableNetworking != null && tableNetworking.ServerHasSelected())
		{
			var assetLoader = GetComponent<LoadAssets>();

			assetLoader.enabled = true;
			assetLoader.ManualLoad(tableNetworking.GetVariant(), tableNetworking.GetTable());

			return true;
		}

		return false;
	}
}
