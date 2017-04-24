using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXBall : MonoBehaviour {
    public AudioClip wallBounce;
    public AudioClip paddleBounce;
    private bool TrueForKyle3wynn;
    public AudioClip[] lBumper;
    public AudioClip[] lPaddle;
    public AudioClip rampUpShort;
    public AudioClip rampUpLong;
    public AudioClip rampDownShort;
    public AudioClip rampDownLong;
    public AudioClip powerUp;
    public AudioClip goalUp;
    public AudioClip goalDown;


    void Start()
    {
        TrueForKyle3wynn = SoundManager.instance.trueForKyle3Wynn;
    }


    void OnCollisionEnter(Collision col)
    {
        var tag = col.gameObject.tag;

        switch (tag)
        {  
            case "Wall":
                SoundManager.instance.RandomizeSfx(wallBounce);

                break;
            case "Paddle":
                if (TrueForKyle3wynn == true)
                {
                    SoundManager.instance.RandomizeSfx(paddleBounce);
                }
                else
                {
                    SoundManager.instance.RandomSFX(lPaddle);
                }
                break;
            case "Flipper":
                SoundManager.instance.RandomizeSfx(wallBounce);
                break;
            case "Bumper":
                SoundManager.instance.RandomSFX(lBumper);

                break;
            case "Goal":
                SoundManager.instance.PlaySingle(goalUp);

                break;
            case "RampUp":
                SoundManager.instance.PlaySingle(rampUpShort);

                break;
            case "RampDown":
                SoundManager.instance.PlaySingle(rampDownShort);

                break;
            case "PowerUp":
                SoundManager.instance.PlaySingle(powerUp);

                break;
            default:
                Debug.Log("Collided with untagged object");
                break;
        }



//        if (col.gameObject.tag == "Wall")
//
//        {
//
//            SoundManager.instance.RandomizeSfx(wallBounce);
//        }
//        else if (col.gameObject.tag == "Paddle")
//        {
//
//            SoundManager.instance.RandomizeSfx(paddleBounce);
//        }
    }

}
