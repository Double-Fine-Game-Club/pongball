using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpinnerActivator : MonoBehaviour
{
    public bool boostEnabled;
    public GameObject boost;

    private void Start()
    {
        boost.GetComponent<BoostPadForce>().boostEnabled = false;
        boost.GetComponent<BoostPad>().lightDisabled();
    }

    private void Awake()
    {
        GetComponent<ObstacleNetworking>().ActivateFromServer += ActivateBoost;
        GetComponent<ObstacleNetworking>().DeactivateFromServer += DeactivateBoost;
    }

    void OnCollisionEnter(Collision col)
    {
        if (!NetworkManager.singleton.isNetworkActive || NetworkServer.connections.Count > 0)
        {
            //Host only
            if (boostEnabled == false)
            {
                ActivateBoost();
                GetComponent<ObstacleNetworking>().ActivateFromServer();
            }
            else if (boostEnabled == true)
            {
                DeactivateBoost();
                GetComponent<ObstacleNetworking>().DeactivateFromServer();
            }
        }
    }

    void DeactivateBoost()
    {
        boost.GetComponent<BoostPadForce>().boostEnabled = false;
        boost.GetComponent<BoostPad>().lightDisabled();
        boostEnabled = false;
    }

    void ActivateBoost()
    {
        boost.GetComponent<BoostPadForce>().boostEnabled = true;
        boost.GetComponent<BoostPad>().lightEnabled();
        boostEnabled = true;
    }
}
