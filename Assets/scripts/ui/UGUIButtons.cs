//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UGUIButtons : MonoBehaviour {
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject aboutPanel;

    void Awake()
    {
        
    }

    public void PlayGameButton()
    {
        SceneManager.LoadScene("BaseTable");
    }

    //This will open the panel if it's closed and close it if it's open
    public void OptionsButton()
    {

        if (optionsPanel.activeSelf == false)
        {
            CloseAllPanels();
            optionsPanel.SetActive(true);

        }
        else if (optionsPanel.activeSelf == true)
        {
            optionsPanel.SetActive(false);
        }
	}

    public void CreditsButton()
    {
        if (creditsPanel.activeSelf == false)
        {
            CloseAllPanels();
            creditsPanel.SetActive(true);
        }
        else if (creditsPanel.activeSelf == true)
        {
            creditsPanel.SetActive(false);
        }
    }

    public void AboutButton()
    {
        if (aboutPanel.activeSelf == false)
        {
            CloseAllPanels();
            aboutPanel.SetActive(true);
        }
        else if (aboutPanel.activeSelf == true)
        {
            aboutPanel.SetActive(false);
        }
    }

    public void QuitButton()
    {
        //this should probably open a menu confirming your selection
        Application.Quit();
    }

    private void CloseAllPanels()
    {
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }
}
