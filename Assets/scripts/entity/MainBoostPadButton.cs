using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBoostPadButton : MonoBehaviour
{
    public bool directionIsForward;
    public GameObject boostPad;

    private void Awake()
    {
        GetComponent<ObstacleNetworking>().ActivateFromServer += OperateBoostPad;
        GetComponent<ObstacleNetworking>().DeactivateFromServer += OperateBoostPad;
    }
    void OnCollisionEnter(Collision col)
    {
        OperateBoostPad();
        GetComponent<ObstacleNetworking>().ActivateOnServer();
    }

    void OperateBoostPad()
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
