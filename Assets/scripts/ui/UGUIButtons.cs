//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UGUIButtons : MonoBehaviour {

    public void PlayGameButton()
    {
        SceneManager.LoadScene("BaseTable");
    }

    public void OptionsButton()
    {
        Debug.Log("This does nothing currently");
    }

    public void QuitButton()
    {
        //this should open a menu confirming your selection
        Application.Quit();
    }

}
