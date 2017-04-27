using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    public AudioSource musicSource;					
    public AudioClip[] musicArray;
    public bool musicIsPlaying = true;
    public int currentSong;

    void Start()
    {
        musicArray = Resources.LoadAll<AudioClip>("audio/music");
        RandomSong();
        StartCoroutine(PlayMusic());
    }

    void Update()
    {
        if (Input.GetKeyDown("n"))
        {
            PlayNextSong();
        }

    }

    public void RandomSong()
    {
        currentSong = Random.Range(0, musicArray.Length);
        //Debug.Log("current song is " + currentSong);
    }


    IEnumerator PlayMusic()
    {
        while (musicIsPlaying == true)
        {
            musicSource.clip = musicArray[currentSong];
            musicSource.Play();
            yield return new WaitForSeconds(musicSource.clip.length);
            if (currentSong < musicArray.Length - 1)
            {
                currentSong++;
            }
            else
            {
                currentSong = 0;
            }
        }
    }

    public void PlayNextSong()
    {
        StopCoroutine(PlayMusic());
        if (currentSong < musicArray.Length - 1)
        {
            currentSong++;
        }
        else
        {
            currentSong = 0;
        }
        StartCoroutine(PlayMusic());

    }

    public void StartMusic()
    {
        StartCoroutine(PlayMusic());
    }

    public void StopMusic()
    {
        musicIsPlaying = false;
        //can also use StopCoroutine(PlayMusic());
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

}

