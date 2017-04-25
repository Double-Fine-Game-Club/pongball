using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoostPadButton : MonoBehaviour
{
    public bool directionIsForward;
    public GameObject boostPad;

    void OnCollisionEnter(Collision col)
    {
        if (directionIsForward == true)
        {
            boostPad.GetComponent<BoostPadOperator>().MoveBoostPadForward();
        }
        else if (directionIsForward == false)
        {
            boostPad.GetComponent<BoostPadOperator>().MoveBoostPadBackward();
        }
    }

}
