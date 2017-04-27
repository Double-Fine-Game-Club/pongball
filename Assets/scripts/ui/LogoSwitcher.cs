using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoSwitcher : MonoBehaviour 
{
    public GameObject[] logoArray;
    private int randomLogo;
    private int currentLogo = 0;
	public float timer = 0;

    void Start()
    {
        RandomLogo();
        //Debug.Log(logoArray.Length);
    }

	public void Update(){
		//update logo switcher
		timer += Time.deltaTime;
		if(timer > 10){
			NextLogo();
			timer = 0;
		}
	}

    public void RandomLogo()
    {
        logoArray[currentLogo].SetActive(false);
        randomLogo = Random.Range(0, logoArray.Length);
        //Debug.Log("randomLogo is " + randomLogo);
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
