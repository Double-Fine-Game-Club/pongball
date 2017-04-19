using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXUI : MonoBehaviour {
    public AudioClip hoverOver;
    public AudioClip onClick;
    public void HoverOverSFX()
    {
        SoundManager.instance.PlaySingle(hoverOver);
    }
    public void OnClickSFX()
    {
        SoundManager.instance.PlaySingle(onClick);
    }

}
