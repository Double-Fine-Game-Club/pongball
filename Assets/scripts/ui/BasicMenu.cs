using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Hacky menu system until we have a proper UI
/// </summary>

public class BasicMenu : MonoBehaviour
{
    enum Menu
    {
        /// <summary>
        /// Initial menu for picking online/offline
        /// </summary>
        MainMenu,

        /// <summary>
        /// Search for online games/join a game
        /// </summary>
        Multiplayer,

        /// <summary>
        /// Select level and variant layout
        /// </summary>
        LevelSelect,

        /// <summary>
        /// Loading the selected level
        /// </summary>
        LoadingLevel,

        /// <summary>
        /// Setup the table
        /// </summary>
        SetupTable,

        /// <summary>
        /// Selecting which player(s) to control
        /// </summary>
        PaddleSelect,

        Done,   // needed for OnGui timing
    };

    Menu currentMenu;
    Menu nextMenu;

    public GameObject[] playerPrefabs;

    void Start()
    {
        currentMenu = Menu.MainMenu;
        nextMenu = Menu.LevelSelect;
    }

    void OnGUI()
    {
        switch (currentMenu)
        {
            case Menu.MainMenu:
                {
                    // GUI Padding
                    GUILayout.Space(100);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.BeginVertical();

                    // Header
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Pick your multiplayer type:");
                    GUILayout.EndHorizontal();

                    // GUI Padding
                    GUILayout.Space(15);

                    // Get player choice
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Toggle(nextMenu == Menu.Multiplayer, "") | GUILayout.Button("Online"))
                    {
                        nextMenu = Menu.Multiplayer;
                    }
                    if (GUILayout.Toggle(nextMenu == Menu.LevelSelect, "") | GUILayout.Button("Local"))
                    {
                        nextMenu = Menu.LevelSelect;
                    }
                    GUILayout.EndHorizontal();

                    // GUI Padding
                    GUILayout.Space(15);

                    // Proceed to next step
                    if (GUILayout.Button("Proceed"))
                    {
                        currentMenu = nextMenu;
                    }

                    // End GUI Padding
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                break;


            case Menu.Multiplayer:
                {
                    // Show the multiplayer menu
                    if (NetworkManager.singleton.GetComponent<NetworkManagerHUD>().enabled == false)
                    {
                        NetworkManager.singleton.GetComponent<NetworkManagerHUD>().enabled = true;
                    }

                    // GUI Padding
                    GUILayout.Space(200);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(15);
                    // Show the table select menu if the network is active
                    if (GUILayout.Button("Proceed"))
                    {
                        if (NetworkManager.singleton.isNetworkActive)
                        {
                            // Hide the networking HUD
                            NetworkManager.singleton.GetComponent<NetworkManagerHUD>().enabled = false;

							if (NetworkServer.connections.Count > 0)
							{
								GameObject tableManager = GameObject.Instantiate(NetworkManager.singleton.spawnPrefabs[7]);
								NetworkServer.Spawn(tableManager);
							}
							
                            currentMenu = Menu.LevelSelect;
                            nextMenu = Menu.LoadingLevel;
                        }
                    }

                    GUILayout.EndHorizontal();
                }
                break;


            case Menu.LevelSelect:
                {
					if (!NetworkManager.singleton.isNetworkActive || NetworkServer.connections.Count > 0)
					{
						// Enable the asset loader and let it load the level async
	                    var assetLoader = GetComponent<LoadAssets>();
	                    Debug.Assert(assetLoader != null, "Missing asset loader!");

	                    assetLoader.enabled = true;

	                    // We want to know when it's finished
	                    assetLoader.OnFinished += FinishedLoading;

	                    currentMenu = Menu.LoadingLevel;
	                    nextMenu = Menu.SetupTable;
					}
					// If this is a client wait until the server has selected a table and use it
					else
					{
						DrawCentered();
						GUILayout.Label("The host is selecting a table...");
						UndoCentered();

                        TableNetworking tableNetworking = null;
                        GameObject g = GameObject.FindGameObjectWithTag("TableNetworking");
                        if (g)
                        {
                            tableNetworking = g.GetComponent<TableNetworking>();
                        }
						
						if (tableNetworking != null && tableNetworking.ServerHasSelected())
						{
							var assetLoader = GetComponent<LoadAssets>();
							
							assetLoader.enabled = true;
							assetLoader.ManualLoad(tableNetworking.GetVariant(), tableNetworking.GetTable());

							// We want to know when it's finished
							assetLoader.OnFinished += FinishedLoading;

							currentMenu = Menu.LoadingLevel;
							nextMenu = Menu.SetupTable;
						}
					}
                }
                break;

            case Menu.LoadingLevel:
                // Just waiting for stuff to happen here
                //DrawCentered();
                //GUILayout.Label("Loading...");
                //UndoCentered();

                break;

            case Menu.SetupTable:
                {
                    // Add the ball spawner script to a spawner in the scene otherwise it will spawn inactive
                    GameObject[] spawners = GameObject.FindGameObjectsWithTag("BallSpawner");
                    Debug.Assert(spawners.Length > 0, "Need at least one spawner in the scene so we know where to put the ball");
                    GameObject spawner = spawners[Random.Range(0, spawners.Length - 1)];
                    spawner.AddComponent<BallSpawner>().ballPrefab = NetworkManager.singleton.spawnPrefabs[0];// Don't like this at all...
                    spawner.AddComponent<ResetBallUI>();

                    // Fill the table with AIs if offline or the server
                    if (!NetworkManager.singleton.isNetworkActive || NetworkServer.connections.Count > 0)
                    {
                        if (GameObject.FindGameObjectsWithTag("Score").Length <= 0)
                        {
                            GameObject scoreManager = GameObject.Instantiate(NetworkManager.singleton.spawnPrefabs[1]);

                            if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count > 0)
                            {
                                NetworkServer.Spawn(scoreManager);
                            }
                        }

                        int playerIndex = 0;

                        foreach (Transform spawnPosition in NetworkManager.singleton.startPositions)
                        {
                            Transform paddlePos = spawnPosition;
                            int paddleIndex = Mathf.FloorToInt(Random.Range(0, playerPrefabs.Length) );
                            var paddlePrefab = playerPrefabs[paddleIndex];
                            GameObject paddle = GameObject.Instantiate(paddlePrefab, paddlePos);
                            if(playerIndex==0)
                            {
                                paddle.AddComponent<PowerManager>();
                            }

                            // Spawn on the clients
                            if (NetworkManager.singleton.isNetworkActive)
                            {
                                NetworkServer.Spawn(paddle);
                                paddle.GetComponent<PaddleNetworking>().SetPaddleIndex(playerIndex);
                            }

                            ++playerIndex;
                        }
                    }

                    currentMenu = Menu.PaddleSelect;
                    nextMenu = Menu.Done;
                }
                break;

            case Menu.PaddleSelect:
                {
                    try
                    {
                        GUILayout.BeginHorizontal();
                    }
                    catch
                    {
                        //Changing the GUI creates an error here
                        //  so we must wait until next repaint to draw 
                        //  gui elements
                        break;
                    }
                    // GUI Padding
                    GUILayout.Space(15);
                    GUILayout.BeginVertical();
                    // GUI Padding
                    GUILayout.Space(15);

                    // Header
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Select paddles to control:");
                    GUILayout.EndHorizontal();

                    // GUI Padding
                    GUILayout.Space(15);

                    // Get player choice
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Paddles: ");

                    SimpleAI[] bots = GameObject.FindObjectsOfType<SimpleAI>();
                    int paddleIndex = 1;

                    // Show a toggle box for each paddle in the scene based on whether it is AI controlled
                    foreach (SimpleAI bot in bots)
                    {
                        // By default use the local state of the AI component
                        bool isBot = bot.enabled;

                        // When networking use the server state
                        if (NetworkManager.singleton.isNetworkActive)
                        {
                            isBot = bot.GetComponent<PaddleNetworking>().IsAIControlled();
                        }

                        // Select whether the paddle should be player controlled
                        GUILayout.Toggle(!isBot, "");

                        if (GUILayout.Button("" + paddleIndex.ToString()))
                        {
                            // If on the network update the server information
                            if (NetworkManager.singleton.isNetworkActive)
                            {
                                if (isBot)
                                {
                                    bot.gameObject.GetComponent<PaddleNetworking>().PossessPaddle();
                                    bot.gameObject.GetComponent<Player>().enabled = true;
                                    bot.gameObject.GetComponent<Remote>().enabled = false;
                                }
                                else
                                {
                                    bot.gameObject.GetComponent<PaddleNetworking>().UnPossessPaddle();
                                    bot.gameObject.GetComponent<Player>().enabled = false;
                                }
                            }
                            else
                            {
                                // If local switch whether the paddle is player controlled
                                bot.gameObject.GetComponent<Player>().enabled = isBot;
                                bot.enabled = !isBot;
                                bot.gameObject.GetComponent<Remote>().enabled = false;
                            }
                        }

                        paddleIndex++;
                    }

                    GUILayout.EndHorizontal();

                    // GUI Padding
                    GUILayout.Space(30);

                    // Proceed to next step
                    if (GUILayout.Button("Proceed"))
                    {
                        // Disable us as we're done here
                        currentMenu = Menu.Done;
                        this.enabled = false;
                    }

                    // End GUI Padding
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                break;
        }
    }


    /// <summary>
    /// Called when the scene has finished loading in
    /// </summary>
    /// <param name="success"></param>
    void FinishedLoading(bool success)
    {
        Debug.Assert(success, "Failed to load level");

        currentMenu = nextMenu;
        nextMenu = Menu.MainMenu;

        // Remove this delegate
        GetComponent<LoadAssets>().OnFinished -= FinishedLoading;

        NetworkServer.SpawnObjects();
    }


    /// <summary>
    /// Utility functions for drawing at the center of the screen
    /// </summary>
    /// <param name="text"></param>
    void DrawCentered()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
    }
    void UndoCentered()
    {
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }

	public void OnClientConnect(NetworkConnection conn)
	{

	}

}
