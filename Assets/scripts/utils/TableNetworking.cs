using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TableNetworking : NetworkBehaviour 
{
	[SyncVar]
	public string variant;

	[SyncVar]
	public string table;

	[SyncVar]
	public bool selected;

	public float logPingFrequency = 5.0f;

	[ServerCallback]
	void Start () 
	{
		selected = false;
		table = "";
		variant = "";

		InvokeRepeating("LogPing", 0.0f, logPingFrequency);
	}

	void OnClientConnect(NetworkConnection conn)
	{
		InvokeRepeating("LogPing", 0.0f, logPingFrequency);
	}

	void LogPing()
	{
		foreach (NetworkClient conn in NetworkClient.allClients)
		{
			Debug.Log("Ping for connection " + conn.connection.address.ToString() + ": " + conn.GetRTT().ToString() + " (ms)");
		}
	}

	public string GetTable()
	{
		return table;
	}

	public string GetVariant()
	{
		return variant;
	}

	public bool ServerHasSelected()
	{
		return selected;
	}

	public void SetTableInfo(string serverVariant, string serverTable)
	{
		selected = true;
		variant = serverVariant;
		table = serverTable;
	}
}
