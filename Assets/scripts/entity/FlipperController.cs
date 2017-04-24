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

	JointSpring spring;

    private JointLimits limits;
 
    void  Awake (){
        GetComponent<HingeJoint>().useSpring = true;

		// Callback to the activate/deactivate methods from server
		GetComponent<ObstacleNetworking>().ActivateFromServer += ActivateFlipper;
		GetComponent<ObstacleNetworking>().DeactivateFromServer += DeactivateFlipper;

		spring = new JointSpring();
    }

   
    void  Update (){

        spring.spring = flipperStrength;
        spring.damper = flipperDamper;

		if (Input.GetButtonDown(inputButtonName))
        {
			ActivateFlipper();

			// Activate the flipper on all clients
			GetComponent<ObstacleNetworking>().ActivateOnServer();
        }
		else if (Input.GetButtonUp(inputButtonName))
        {
			DeactivateFlipper();

			// Deactivate the flipper on all clients
			GetComponent<ObstacleNetworking>().DeactivateOnServer();
        }

        GetComponent<HingeJoint>().spring = spring;
        GetComponent<HingeJoint>().useLimits = true;
        limits = GetComponent<HingeJoint>().limits;
        limits.min = restPosition;
        limits.max = pressedPosition;
        GetComponent<HingeJoint>().limits = limits;
    }

	void ActivateFlipper()
	{
		spring.targetPosition = pressedPosition;
	}

	void DeactivateFlipper()
	{
		spring.targetPosition = restPosition;
	}


}
