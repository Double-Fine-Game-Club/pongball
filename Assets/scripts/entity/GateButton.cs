using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour 
{
    public bool directionIsForward;
    public GameObject gate;

    void Awake()
    {
            GetComponent<ObstacleNetworking>().ActivateFromServer += OperateGate;
            GetComponent<ObstacleNetworking>().DeactivateFromServer += OperateGate;
    }

    void OnCollisionEnter(Collision col)
    {
        OperateGate();
        GetComponent<ObstacleNetworking>().ActivateOnServer();
    }
    void OperateGate()
    {
        if (directionIsForward == true)
        {
            gate.GetComponent<GateOperator>().MoveGateForward();
        }
        else if (directionIsForward == false)
        {
            gate.GetComponent<GateOperator>().MoveGateBackward();
        }
    }

}
