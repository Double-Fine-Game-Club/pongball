using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForceToBall : MonoBehaviour {

    private float force = 10.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            Rigidbody rigidBody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 forceVector = rigidBody.transform.forward.normalized;
            forceVector.y = 0;
            rigidBody.AddRelativeForce(forceVector * force);
        }
    }
}
