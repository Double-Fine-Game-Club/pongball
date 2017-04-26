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
    public GameObject instructionPanel;
	public GameObject clientWaitPanel;
	public GameObject backgroundPanel;
    
    [SerializeField]
    private Text instruction;

    bool offlineSelected;

    // Use this for initialization
    void Start () {
        offlineSelected = false;
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
        offlineSelected = true;

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

    public void InstructionPanel()
    {
        if (instructionPanel.activeSelf == false)
        {
            CloseAllPanels();
            instructionPanel.SetActive(true);
        }
        else if (instructionPanel.activeSelf == true)
        {
            instructionPanel.SetActive(false);
        }

        instruction.text = "Score goals and earn the most points! ";
        instruction.text += "The points given for each goal (the number at the top) increases as the ball hits bumpers or rolls over lightpads. ";
        instruction.text += "A different power is given to each player every 20 seconds.\n\n";

        if(offlineSelected)
        {
            instruction.text += "RIGHT PLAYER ";
        }

        instruction.text += "CONTROLS:\n";
        instruction.text += "Down = move up\n";
        instruction.text += "Down = move down\n";
        instruction.text += "Space = hit animation\n";
        instruction.text += "Left Alt = activate power\n\n";

        if(offlineSelected)
        {
            instruction.text += "LEFT PLAYER CONTROLS:\n";
            instruction.text += "Y = move up\n";
            instruction.text += "H = move down\n";
            instruction.text += "Q = hit animation\n";
            instruction.text += "Z = activate power\n";
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
        instructionPanel.SetActive(false);
    }

	public void CloseUI()
	{
		backgroundPanel.SetActive(false);
	}
}
