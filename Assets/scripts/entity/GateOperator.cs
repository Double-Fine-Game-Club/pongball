using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOperator : MonoBehaviour {
    public bool directionIsForward;
    public GameObject gate;
    public float gateOpenAngle = 90.0F;
    public float gateCloseAngle = 0.0F;
    public float smoothMovement = 2.0F;

    void OnCollisionEnter(Collision col)
    {

        if (directionIsForward == true)

        {
            var target = Quaternion.Euler (0, gateOpenAngle, 0);
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target,
                Time.deltaTime * smoothMovement);

        }
        else if (directionIsForward == false)
        {
            var target1 = Quaternion.Euler (0, gateCloseAngle, 0);
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target1,
                Time.deltaTime * smoothMovement);

        }
    }

    //For Testing
    void Update()
    {
        if (directionIsForward == true)

        {
            var target = Quaternion.Euler (0, gateOpenAngle, 0);
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target,
                Time.deltaTime * smoothMovement);

        }
        else if (directionIsForward == false)
        {
            var target1 = Quaternion.Euler (0, gateCloseAngle, 0);
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target1,
                Time.deltaTime * smoothMovement);

        }
    }

}
