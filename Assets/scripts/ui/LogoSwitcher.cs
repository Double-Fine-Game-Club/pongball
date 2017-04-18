using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoSwitcher : MonoBehaviour 
{
    public GameObject[] logoArray;
    private int randomLogo;
    private int currentLogo = 0;

    void Start()
    {
        RandomLogo();
        Debug.Log(logoArray.Length);
    }

    public void RandomLogo()
    {
        randomLogo = Random.Range(0, logoArray.Length-1);
        logoArray[randomLogo].SetActive(true);
        currentLogo = randomLogo;

    }

    public void NextLogo()
    {
        logoArray[currentLogo].SetActive(false);
        if (currentLogo < logoArray.Length-1)
        {
        currentLogo++;
        }
        else
        {
            currentLogo = 0;
        }
        logoArray[currentLogo].SetActive(true);


       
    }
}
