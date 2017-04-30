using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerActivator : MonoBehaviour
{
    public bool boostEnabled;
    public GameObject boost;

    private void Start()
    {
        boost.GetComponent<BoostPadForce>().boostEnabled = false;
        boost.GetComponent<BoostPad>().lightDisabled();

        GetComponent<ObstacleNetworking>().ActivateFromServer += ActivateBoost;
        GetComponent<ObstacleNetworking>().DeactivateFromServer += DeactivateBoost;
    }

    void OnCollisionEnter(Collision col)
    {
        if (boostEnabled == true)
        {
            ActivateBoost();
            GetComponent<ObstacleNetworking>().ActivateFromServer();
        }
        else if (boostEnabled == false)
        {
            DeactivateBoost();
            GetComponent<ObstacleNetworking>().DeactivateFromServer();
        }
    }

    void ActivateBoost()
    {
        boost.GetComponent<BoostPadForce>().boostEnabled = false;
        boost.GetComponent<BoostPad>().lightDisabled();
        boostEnabled = false;
    }

    void DeactivateBoost()
    {
        boost.GetComponent<BoostPadForce>().boostEnabled = true;
        boost.GetComponent<BoostPad>().lightEnabled();
        boostEnabled = true;
    }
}
