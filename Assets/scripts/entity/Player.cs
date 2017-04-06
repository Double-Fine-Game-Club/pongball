using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private float Thrust = 10.0f;

    private Vector3 up = new Vector3(1, 0, 0);
    private Vector3 down = new Vector3(-1, 0, 0);

    private Rigidbody rigidBody;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float haxis = Input.GetAxis("Horizontal");
        float vaxis = Input.GetAxis("Vertical");

        if(haxis != 0)
        {

        }

        if(vaxis != 0)
        {
            vaxis *= -1;
            //rigidBody.AddForce(up * (Thrust * vaxis));
            transform.Translate(up * (Thrust * vaxis) * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {

    }

    private void MoveUp()
    {
        if (rigidBody)
        {
            rigidBody.AddForce(up * Thrust);
        }
    }

    private void MoveDown()
    {
        if (rigidBody)
        {
            rigidBody.AddForce(down * Thrust);
        }
    }
}
