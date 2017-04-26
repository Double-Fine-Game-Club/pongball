using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuHandlerUGUI : MonoBehaviour {
    /// <summary>
    /// This script opens and closes panels when buttons are clicked.  That's it's job. - sjm
    /// </summary>
    public GameObject localOrOnlinePanel;
    public GameObject singleOrMultiPanel;
    public GameObject onlinePanel;
	public GameObject levelSelectionPanel;
	public GameObject clientWaitPanel;
	public GameObject backgroundPanel;


	// Use this for initialization
	void Start () {
		
	}


    //merp was going to do something better, like this, but decided the other way was quicker to program, atm. - sjm
    void Function(string menu)
    {
    }


    public void OnlineOrLocalPanel()
    {

        if (localOrOnlinePanel.activeSelf == false)
        {
            CloseAllPanels();
            localOrOnlinePanel.SetActive(true);

        }
        else if (localOrOnlinePanel.activeSelf == true)
        {
            localOrOnlinePanel.SetActive(false);
        }
    }

    public void LocalPanel()
    {
        if (singleOrMultiPanel.activeSelf == false)
        {
            CloseAllPanels();
            singleOrMultiPanel.SetActive(true);
        }
        else if (singleOrMultiPanel.activeSelf == true)
        {
            singleOrMultiPanel.SetActive(false);
        }
    }


    public void OnlinePanel()
    {
        if (onlinePanel.activeSelf == false)
        {
            CloseAllPanels();
            onlinePanel.SetActive(true);
        }
        else if (onlinePanel.activeSelf == true)
        {
            onlinePanel.SetActive(false);
        }
    }

    public void LevelSelectionPanel()
    {
        if (levelSelectionPanel.activeSelf == false)
        {
            CloseAllPanels();
            levelSelectionPanel.SetActive(true);
        }
        else if (levelSelectionPanel.activeSelf == true)
        {
            levelSelectionPanel.SetActive(false);
        }
    }

	public void ClientWaitPanel()
	{
		if (clientWaitPanel.activeSelf == false)
		{
			CloseAllPanels();
			clientWaitPanel.SetActive(true);
		}
		else if (clientWaitPanel.activeSelf == true)
		{
			clientWaitPanel.SetActive(false);
		}
	}
	
    private void CloseAllPanels()
    {
        localOrOnlinePanel.SetActive(false);
        singleOrMultiPanel.SetActive(false);
        onlinePanel.SetActive(false);
        levelSelectionPanel.SetActive(false);
    }

	public void CloseUI()
	{
		backgroundPanel.SetActive(false);
	}
}
