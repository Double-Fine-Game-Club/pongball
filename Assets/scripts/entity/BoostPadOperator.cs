using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPadOperator : MonoBehaviour
{
    public bool directionIsForward;
    public GameObject boostPad;
    //public float forwardAngle = 245.0F;
    //public float restingPosition = 0.0F;
    //public float backwardAngle = -245.0F;
    //public float smoothMovement = 10.0F;


    public void MoveBoostPadForward()
    {
        var target = Quaternion.Euler(0, 0, 0);
        // Dampen towards the target rotation
        transform.localRotation = target;
            //Time.deltaTime * smoothMovement);
    }

    public void MoveBoostPadBackward()
    {
        var target1 = Quaternion.Euler(0, 180, 0);
        // Dampen towards the target rotation
        transform.localRotation = target1;
            //Time.deltaTime * smoothMovement);
    }

    //public void MoveGateResting()
    //{
       // var target1 = Quaternion.Euler(0, restingPosition, 0);
        // Dampen towards the target rotation
        //transform.localRotation = Quaternion.Slerp(transform.localRotation, target1,
            //Time.deltaTime * smoothMovement);
    //}
}