using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXBall : MonoBehaviour {
    public AudioClip wallBounce;
    public AudioClip paddleBounce;

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Wall")

        {

            SoundManager.instance.RandomizeSfx(wallBounce);
        }
        else if (col.gameObject.tag == "Paddle")
        {

            SoundManager.instance.RandomizeSfx(paddleBounce);
        }
    }

}
