using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Should be changed to NetworkBehaviour eventually but doing so currently disables object at runtime when selecting local multiplayer
public class Score : MonoBehaviour // NetworkBehaviour
{
    private int score1;
    private int score2;

    [SerializeField]
    private Text score1Text;

    [SerializeField]
    private Text score2Text;

    // Use this for initialization
    void Start ()
    {
        //if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;

        score1 = 0;
        score2 = 0;
    }

    // I believe the event subscription method wasn't working because this occurs before ball instantitation
    public void OnEnable()
    {
        Debug.Log("Score.OnEnable()");
        Ball.OnTriggerEnterGoal1 += OnTriggerEnterGoal1;
        Ball.OnTriggerEnterGoal2 += OnTriggerEnterGoal2;
    }

    void OnDisable()
    {
        Ball.OnTriggerEnterGoal1 -= OnTriggerEnterGoal1;
        Ball.OnTriggerEnterGoal2 -= OnTriggerEnterGoal2;
    }

    private void FixedUpdate()
    {
        //if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count == 0) return;
    }

    public void OnTriggerEnterGoal1()
    {
        //Debug.Log("Score.OnTriggerEnterGoal1()");
        score1++;
        score1Text.text = score1.ToString();
    }

    public void OnTriggerEnterGoal2()
    {
        //Debug.Log("Score.OnTriggerEnterGoal2()");
        score2++;
        score2Text.text = score2.ToString();
    }
}
