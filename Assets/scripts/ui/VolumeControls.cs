using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControls : MonoBehaviour {
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioSource sfxSource;
    public AudioSource musicSource;

    public void ChangeMusicVolume()
    {
        musicSource.volume = musicSlider.value;
    }

    public void ChangeSFXVolume()
    {
        sfxSource.volume = sfxSlider.value;
    }
}
