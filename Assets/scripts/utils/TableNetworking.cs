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

	[ServerCallback]
	void Start () 
	{
		selected = false;
		table = "";
		variant = "";
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
