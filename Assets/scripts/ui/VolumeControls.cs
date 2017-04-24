using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControls : MonoBehaviour {
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public Toggle kyle;
    public Toggle lclhster;

    /*Awake() get's called when the scene opens even if it's on a dontdestroyonload object
     * But I'm not doing that atm
     * void Awake()
    {

    }*/

    void Start()
    {
        /*Playerprefs doesn't store bools.  I'm leaving this here because I'm a code hoarrder - sjm
        if (PlayerPrefs.HasKey("trueForKyle3Wynn"))
        {
            SoundManager.instance.trueForKyle3Wynn =  PlayerPrefs.Set
        }
        */

        //sets the toggles when the scene loads
        if (SoundManager.instance.trueForKyle3Wynn == true)
        {
            kyle.isOn = true;
            lclhster.isOn = false;
        }
        else
        {
            kyle.isOn = false;
            lclhster.isOn = true;
        }



        sfxSource = SoundManager.instance.efxSource;
        musicSource = SoundManager.instance.musicSource;
        if (PlayerPrefs.HasKey("soundVolume"))
        {
            sfxSource.volume = PlayerPrefs.GetFloat("soundVolume");
            musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
        }

        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;
    }

    public void ChangeMusicVolume()
    {
        musicSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }

    public void ChangeSFXVolume()
    {
        sfxSource.volume = sfxSlider.value;
        PlayerPrefs.SetFloat("soundVolume", sfxSlider.value);

    }

    //I know there's probably a better way to do this but I'm sleepy :/ - sjm
    public void Kyle3wynn()
    {
        SoundManager.instance.trueForKyle3Wynn = true;

    }

    public void Lclhoster()
    {
        SoundManager.instance.trueForKyle3Wynn = false;

    }

}
