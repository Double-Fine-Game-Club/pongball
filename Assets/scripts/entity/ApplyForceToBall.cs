using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForceToBall : MonoBehaviour {

    private float force = 10.0f;

    private Vector3 myStartingScale = Vector3.zero;
    private float scaleFactor = 1.2f;
    private float lerpAmount = 0.05f;

    private void Start()
    {
        myStartingScale = transform.localScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, myStartingScale, lerpAmount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 forceVector = rb.transform.forward.normalized;
            forceVector.y = 0;
            rb.AddForce(forceVector * force);

            Vector3 newScale = new Vector3(
                myStartingScale.x * scaleFactor,
                myStartingScale.y * scaleFactor,
                myStartingScale.z * scaleFactor);

            transform.localScale = newScale;
        }
    }
}
