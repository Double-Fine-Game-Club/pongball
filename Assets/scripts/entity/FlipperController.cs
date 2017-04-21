using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperController : MonoBehaviour 
{
    public float restPosition = 0F;
    public float pressedPosition = 45F;
    public float flipperStrength = 2000f;
    public float flipperDamper = 5f;
    public string inputButtonName = "LeftFlipper";
    private HingeJoint paddleHingeJoint;

    private JointLimits limits;
 
    void  Awake (){
        GetComponent<HingeJoint>().useSpring = true;
    }

   
    void  Update (){
        JointSpring spring = new JointSpring();

        spring.spring = flipperStrength;
        spring.damper = flipperDamper;

        if (Input.GetButton(inputButtonName))
        {
            spring.targetPosition = pressedPosition;
        }
        else
        {
            spring.targetPosition = restPosition;
        }

        GetComponent<HingeJoint>().spring = spring;
        GetComponent<HingeJoint>().useLimits = true;
        limits = GetComponent<HingeJoint>().limits;
        limits.min = restPosition;
        limits.max = pressedPosition;
        GetComponent<HingeJoint>().limits = limits;
    }


}
