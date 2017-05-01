using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Score : NetworkBehaviour
{
	// Synchronise the variables from the server to the players
	[SyncVar]
	private int currentGoalValue;

	[SyncVar]
	private int score1;

	[SyncVar]
	private int score2;

	[SerializeField]
	private Text score1Text;

	[SerializeField]
	private Text score2Text;

	[SerializeField]
	private Text ballValueText;

    bool isHost = true;
	private Coroutine countdownCoroutine = null;
	public Text CountdownText;
	private int baseGoalValue = 50;
	private int bumperValue = 5;
	private int rolloverValue = 1;

    private int gameWinValue = 300;
    private bool gameWinState = false;
    private float gameWinCountdown = 0; // Wait a while when showing game win text

    public delegate void TriggerResetObstaclesDelegate();
    public static event TriggerResetObstaclesDelegate OnTriggerResetObstacles;

    // Use this for initialization
    void Start ()
	{
		score1 = 0;
		score2 = 0;
		currentGoalValue = baseGoalValue;
	}

	// I believe the event subscription method wasn't working because this occurs before ball instantitation
	public void OnEnable()
	{
        isHost = (!NetworkManager.singleton.isNetworkActive || NetworkServer.connections.Count > 0);
		Ball.OnTriggerReset += OnTriggerReset;
		if (!isHost) return;

		Debug.Log("Score.OnEnable()");
		Ball.OnTriggerEnterGoal1 += OnTriggerEnterGoal1;
		Ball.OnTriggerEnterGoal2 += OnTriggerEnterGoal2;
		Ball.OnTriggerEnterBumper += OnTriggerEnterBumper;
		Ball.OnTriggerEnterRollover += OnTriggerEnterRollover;

	}

	void OnDisable()
	{
		Ball.OnTriggerReset -= OnTriggerReset;
		if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

		Ball.OnTriggerEnterGoal1 -= OnTriggerEnterGoal1;
		Ball.OnTriggerEnterGoal2 -= OnTriggerEnterGoal2;
		Ball.OnTriggerEnterBumper -= OnTriggerEnterBumper;
		Ball.OnTriggerEnterRollover -= OnTriggerEnterRollover;
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

		if (int.Parse(ballValueText.text) != currentGoalValue)
		{
			ballValueText.text = currentGoalValue.ToString();
		}
	}

    private void Update()
    {
        CheckGameWinState();
    }

    private void CheckGameWinState()
    {
        //if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

        //
        // TODO: Very rudimentary implementation of a game win state. Fix.
        //
        if ((score1 >= gameWinValue || score2 >= gameWinValue) && gameWinState == false)
        {
            // Set game win state
            gameWinState = true;

            //Setting timescale to zero prevents fixedupdate from being called and score UI from being updated. Let's force that now
            FixedUpdate();

            Time.timeScale = 0;

            // Update and show win text
            CountdownText.enabled = true;
            CountdownText.color = Color.white; // fix for this getting very transparent at times
            CountdownText.text = (score1 > score2 ? "Left" : "Right") + " Player wins!";

            // Pause all game objects
            UnityEngine.Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
            {
                go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
            }

            // Set countdown timer
            gameWinCountdown = 100;
        }
        else if (gameWinState)
        {
            // Show the win text for a while before accepting input
            if(gameWinCountdown > 0)
            {
                gameWinCountdown -= 1;
                return;
            }

            // should only update once
			CountdownText.text = (score1 > score2 ? "Left" : "Right") + " Player wins!";

            if (isHost)
            {
                CountdownText.text += "\nPress any key to continue";

                if (Input.anyKey)
                {
                    Time.timeScale = 1;

                    // Destroy all active balls
                    List<GameObject> balls = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ball"));
                    balls.ForEach(b => Destroy(b));

                    // Reset score
                    score1 = score2 = 0;

                    // Hide coutdown text
                    CountdownText.enabled = false;

                    // Unpause all game objects
                    UnityEngine.Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
                    foreach (GameObject go in objects)
                    {
                        go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
                    }

                    // Unset game win state
                    gameWinState = false;

                    // Trigger a reset
                    OnTriggerReset();
                }
            }
        }
        if(gameWinState && score1==0 && score2==0)
        {
            //End Win state on client
            //score is network updated but winstate is not
            Time.timeScale = 1;
            gameWinState = false;
            CountdownText.text = "3";
        }
    }

    // Only increase the score on the server or in local
    public void OnTriggerEnterGoal1()
	{
		if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

		//Debug.Log("Score.OnTriggerEnterGoal1()");
		score1+= currentGoalValue;
		currentGoalValue = baseGoalValue;

        if (OnTriggerResetObstacles != null)
            OnTriggerResetObstacles();

        ResetBall();
	}

	public void OnTriggerEnterGoal2()
	{
		if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

		//Debug.Log("Score.OnTriggerEnterGoal2()");
		score2 += currentGoalValue;
		currentGoalValue = baseGoalValue;

        if (OnTriggerResetObstacles != null)
            OnTriggerResetObstacles();

        ResetBall();
    }

    private void ResetBall()
    {
        // HACK to make win state work
        CheckGameWinState();
        if (gameWinState) return; 
        GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>().ResetBall();
    }

    public void OnTriggerEnterBumper()
	{
		currentGoalValue += bumperValue;
	}

	public void OnTriggerEnterRollover()
	{
		currentGoalValue += rolloverValue;
	}

	private IEnumerator Countdown()
	{
		var originalColor = CountdownText.color;
		CountdownText.enabled = true;
		for (int count = 3; count > 0; count--)
		{
			CountdownText.text = count.ToString();
			yield return new WaitForSeconds(1f);
		}
		CountdownText.text = "GO!";
		yield return new WaitForSeconds(0.5f);

		var fadedColor = originalColor;
		var steps = 10;
		var fadeDuration = 0.5f;
		for (int step = 0; step < steps; step++)
		{
			var dt = 1f / (float)steps;
			var t = (float)step * dt;
			fadedColor.a = originalColor.a * (1f - t);
			CountdownText.color = fadedColor;
			yield return new WaitForSeconds(fadeDuration * dt);
		}
		CountdownText.enabled = false;
		CountdownText.color = originalColor;
		countdownCoroutine = null;
	}

	public void OnTriggerReset()
	{
        if (gameWinState) return;

		if (countdownCoroutine != null)
		{
			StopCoroutine(countdownCoroutine);
		}
        countdownCoroutine = StartCoroutine(Countdown());

        if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count > 0)
        {
            RpcClientCountdown();
        }

    }

    [ClientRpc]
    void RpcClientCountdown()
    {
        StartCoroutine(Countdown());
    }
}
