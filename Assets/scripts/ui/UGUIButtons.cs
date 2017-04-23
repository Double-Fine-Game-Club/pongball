//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UGUIButtons : MonoBehaviour {
    public GameObject optionsPanel;
    public GameObject creditsPanel;



    public void PlayGameButton()
    {
        SceneManager.LoadScene("BaseTable");
    }

    public void OptionsButton()
    {
        if (optionsPanel.activeSelf == false)
        {
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
            creditsPanel.SetActive(true);
            optionsPanel.SetActive(false);
        }
        else if (creditsPanel.activeSelf == true)
        {
            creditsPanel.SetActive(false);
        }
    }

    public void QuitButton()
    {
        //this should open a menu confirming your selection
        Application.Quit();
    }

}
