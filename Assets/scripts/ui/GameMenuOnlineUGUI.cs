using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameMenuOnlineUGUI : MonoBehaviour 
{
    public GameObject hostButton;
    public GameObject joinButton;
    public GameObject ipInputField;
    public GameObject portInputField;

	private NetworkSetup networkSetup;

	void Start()
	{
		networkSetup = GetComponent<NetworkSetup>();

		ipInputField.GetComponent<InputField>().text = NetworkManager.singleton.networkAddress;
		portInputField.GetComponent<InputField>().text = NetworkManager.singleton.networkPort.ToString();
	}

	void Update()
	{
		
	}

    public void HostButton()
	{
		int port = int.Parse(portInputField.GetComponent<InputField>().text);
		networkSetup.BeginHosting(ipInputField.GetComponent<InputField>().text, port);
	}

    public void JoinButton()
	{
		int port = int.Parse(portInputField.GetComponent<InputField>().text);
		networkSetup.ConnectToServer(ipInputField.GetComponent<InputField>().text, port);
    }

	public void CancelButton()
	{
		NetworkManager.singleton.StopHost();
	}
}
