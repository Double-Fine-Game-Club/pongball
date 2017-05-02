using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpinnerActivator : MonoBehaviour
{
    public bool boostEnabled;
    public GameObject boost;
    //I'm onlly making this public so I can watch it in the inspector - sjm
    public bool spinnerCountDown;
    //declaring this to avoid excess garbage allocation
    public WaitForSeconds countTime = new WaitForSeconds(1);

    public Vector3 com;
    public Rigidbody rb;


    private void Start()
    {
        boost.GetComponent<BoostPadForce>().boostEnabled = false;
        boost.GetComponent<BoostPad>().lightDisabled();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;
    }

    private void Awake()
    {
        GetComponent<ObstacleNetworking>().ActivateFromServer += ActivateBoost;
        GetComponent<ObstacleNetworking>().DeactivateFromServer += DeactivateBoost;
        GetComponent<ObstacleNetworking>().ResetFromServer += DeactivateBoost;
    }

    private void OnEnable()
    {
        Score.OnTriggerResetObstacles += OnLevelReset;
    }

    private void OnDisable()
    {
        Score.OnTriggerResetObstacles += OnLevelReset;
    }

    private void OnLevelReset()
    {
        DeactivateBoost();
        GetComponent<ObstacleNetworking>().ResetOnServer();
    }

    void OnCollisionEnter(Collision col)
    {
        if (!NetworkManager.singleton.isNetworkActive || NetworkServer.connections.Count > 0)
        {
            //Host only
            if (spinnerCountDown == false)
            {
                StartCoroutine(SpinnerCountDown());
                if (boostEnabled == false)
                {
                    ActivateBoost();
                    GetComponent<ObstacleNetworking>().ActivateOnServer();
                }
                else if (boostEnabled == true)
                {
                    DeactivateBoost();
                    GetComponent<ObstacleNetworking>().ActivateOnServer();
                }
            }
        }
    }

    //This makes sure that the Spinner trigger is only activated once per second
    IEnumerator SpinnerCountDown()
    {
        spinnerCountDown = true;
        yield return countTime;
        spinnerCountDown = false;            
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
