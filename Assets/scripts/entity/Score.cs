using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Score : NetworkBehaviour
{
	// Synchronise the variables from the server to the players
	[SyncVar]
    private int score1;

	[SyncVar]
    private int score2;

    [SerializeField]
    private Text score1Text;

    [SerializeField]
    private Text score2Text;

    // Use this for initialization
    void Start ()
    {
        score1 = 0;
        score2 = 0;
    }

    // I believe the event subscription method wasn't working because this occurs before ball instantitation
    public void OnEnable()
	{
		if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

        Debug.Log("Score.OnEnable()");
        Ball.OnTriggerEnterGoal1 += OnTriggerEnterGoal1;
        Ball.OnTriggerEnterGoal2 += OnTriggerEnterGoal2;
    }

    void OnDisable()
	{
		if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

        Ball.OnTriggerEnterGoal1 -= OnTriggerEnterGoal1;
        Ball.OnTriggerEnterGoal2 -= OnTriggerEnterGoal2;
    }

    private void FixedUpdate()
	{
		// Update the score text if the scores have changed
		if (int.Parse(score1Text.text) != score1)
		{
			score1Text.text = score1.ToString();
		}

		if (int.Parse(score2Text.text) != score2)
		{
			score2Text.text = score2.ToString();
		}
    }

	// Only increase the score on the server or in local
    public void OnTriggerEnterGoal1()
    {
		if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

        //Debug.Log("Score.OnTriggerEnterGoal1()");
        score1++;
    }

    public void OnTriggerEnterGoal2()
    {
		if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

        //Debug.Log("Score.OnTriggerEnterGoal2()");
        score2++;
    }
}
