using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour 
{
    public bool directionIsForward;
    public GameObject gate;

    void OnCollisionEnter(Collision col)
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
