using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lights up when a ball triggers it
/// </summary>
public class BoostPad : MonoBehaviour
{

    public Color baseColor;
    public Color lightColor;
    //private int balls;
    private float fadeTime;
    public float fadeDuration;

    void Start()
    {
        //balls = 0;
        //fadeTime = 0.0f;
        SetColor(baseColor);
    }

   /* void Update()
    {
        //if (balls <= 0 && GetColor() != baseColor)
        if (GetColor() != baseColor)

        {
            fadeTime = Mathf.Min(fadeTime + Time.deltaTime, fadeDuration);

            SetColor(Color.Lerp(lightColor, baseColor, fadeTime / fadeDuration));
        }
    }*/

    public void lightEnabled()
    {
        SetColor(lightColor);
    }

    public void lightDisabled()
    {
        SetColor(baseColor);
        //fadeTime = 0.0f;
    }

    Color GetColor()
    {
        return GetComponent<Renderer>().material.color;
    }

    void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }
}
