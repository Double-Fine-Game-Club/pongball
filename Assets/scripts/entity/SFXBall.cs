using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXBall : MonoBehaviour {
    public AudioClip wallBounce;
    public AudioClip paddleBounce;

    void onCollisionEnter(Collision col)
    {
        Debug.Log("Am I stupid");
        if (col.gameObject.tag == "Wall")

        {
            Debug.Log("Wall!");
            SoundManager.instance.RandomizeSfx(wallBounce);
        }
        else if (col.gameObject.tag == "Paddle")
        {
            Debug.Log("Paddle!");
            SoundManager.instance.RandomizeSfx(paddleBounce);
        }
    }

}
