using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallSpawner : NetworkBehaviour {

    public GameObject ballPrefab;

    public override void OnStartServer()
    {
        var ball = (GameObject)Instantiate(ballPrefab, transform.position, transform.rotation);
        ball.GetComponent<Rigidbody>().velocity = ball.transform.forward * 5;
        NetworkServer.Spawn(ball);
    }

}
