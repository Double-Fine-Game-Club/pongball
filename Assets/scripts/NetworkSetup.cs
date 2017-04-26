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
		if (GetComponent<LoadAssets>().bundlesLoaded == false && NetworkClient.active && !NetworkServer.active)
		{
			LoadTableOnClient();

			if (GetComponent<LoadAssets>().bundlesLoaded == true)
			{
				// close the wait dialogue
				GetComponent<GameMenuHandlerUGUI>().ClientWaitPanel();

				Debug.Log("Being playing");

				StartCoroutine(GetComponent<LoadAssets>().BeginPlaying());
			}
		}
	}

	// Begin hosting using the given port
	public void BeginHosting(string IP, int port)
	{
		NetworkManager.singleton.networkAddress = IP;
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
	public void ConnectToServer(string IP, int remotePort)
	{
		NetworkManager.singleton.networkAddress = IP;
		NetworkManager.singleton.networkPort = remotePort;

		NetworkManager.singleton.StartClient();
	}

	// Attempts to set up the table on the client (selected by server) returning true if successful
	public void LoadTableOnClient()
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
		}
	}
}
