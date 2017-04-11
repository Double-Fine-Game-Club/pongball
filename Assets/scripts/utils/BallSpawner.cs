using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallSpawner : NetworkBehaviour {

    public GameObject ballPrefab;

    private void Update()
    {
        // HACK: Ball wont respawn when re-hosting...
        if (!isServer) return;

        var ball = GameObject.FindGameObjectWithTag("Ball");
        if(ball == null)
        {
            ball = (GameObject)Instantiate(ballPrefab, transform.position, transform.rotation);
            ball.GetComponent<Rigidbody>().velocity = ball.transform.forward * 5;
            NetworkServer.Spawn(ball);
        }
    }

}
