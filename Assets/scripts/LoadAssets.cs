using AssetBundles;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LoadAssets : MonoBehaviour {
    public string sceneAssetBundle;

    // TODO: Can we display dictionaries in the editor??
    public Dictionary<string, bool> variantNames = new Dictionary<string, bool>() {
		{ "lightsoda", false },
		{ "kgunn", false },
		{ "kednar", false }
	};
	public Dictionary<string, bool> tableNames = new Dictionary<string, bool>() {
		{ "Empty", false },
		{ "Bumpout", false },
		{ "Flippers", false },
		{ "Speedster", false },
		{ "Hillock", false }
	};
	public Dictionary<string, bool> paddleNames = new Dictionary<string, bool>() {
		{ "tron", false },
		{ "robot blue", false },
		{ "robot green", false },
		{ "robot red", false },
		{ "robot yellow", false }
	};

	public GameObject[] playerPrefabs;

	private string[] activeVariants;
	private bool finishedLoading;

	public bool bundlesLoaded;

	public delegate void FinishedCallback(bool success);
	public FinishedCallback OnFinished;

	public bool isSinglePlayer;

    void Awake()
    {
        activeVariants = new string[2];
        bundlesLoaded = false;
		finishedLoading = false;
		isSinglePlayer = true;

        Debug.Assert(variantNames != null, "No variant names assigned");
        Debug.Assert(tableNames != null, "No table names assigned");
    }

    // Creating the Temp UI for the demo in IMGui.
    /*void OnGUI()
    {
        if (!bundlesLoaded)
        {
            // GUI Padding
            GUILayout.Space(200);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginVertical();

            AddGUIButtons();

            // GUI Padding
            GUILayout.Space(15);

            // Load the Scene
            if (GUILayout.Button("Load Scene"))
            {
                // Remove the buttons
                bundlesLoaded = true;
                // Set the activeVariant
                activeVariants[0] = "table-" + GetActiveFromDictionary(variantNames);
                // Show this in the log to make sure it is correct
                Debug.Log(activeVariants[0]);

				if (NetworkManager.singleton.isNetworkActive)
				{
					GameObject.FindGameObjectWithTag("TableNetworking").GetComponent<TableNetworking>().SetTableInfo(GetActiveFromDictionary(variantNames), GetActiveFromDictionary(tableNames));
				}

                // Load the scene now!
                StartCoroutine(BeginExample());
            }

            // End GUI Padding
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }*/

    private void AddGUIButtons()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Select variant:");
        Dictionary<string,bool> kvpCopies = new Dictionary<string, bool>(variantNames);
        foreach (KeyValuePair<string, bool> variant in kvpCopies)
        {
            GUILayout.BeginHorizontal();
            if(GUILayout.Button(variant.Key))
            {
                SetActiveInDictionary(variant.Key, variantNames);
            }
            GUILayout.TextField(variant.Value.ToString());
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Select Table:");
        kvpCopies = new Dictionary<string, bool>(tableNames);
        foreach (KeyValuePair<string, bool> table in kvpCopies)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(table.Key))
            {
                SetActiveInDictionary(table.Key, tableNames);
            }
            GUILayout.TextField(table.Value.ToString());
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private void SetActiveInDictionary(string activeKey, Dictionary<string, bool> dict)
    {
        List<string> keys = new List<string>(dict.Keys);
        foreach (string key in keys)
            dict[key] = false;

        dict[activeKey] = true;
    }

    private string GetActiveFromDictionary(Dictionary<string, bool> dict)
    {
        string selectedValue = "";

        foreach (KeyValuePair<string, bool> kvp in dict)
        {
            if (kvp.Value) selectedValue = kvp.Key;
        }

        Debug.Assert(selectedValue.Length > 0, "Returning empty selected value from dictionary");

        return selectedValue;
    }

    // Use this for initialization
    IEnumerator BeginExample()
    {
        yield return StartCoroutine(Initialize());

        // Set active variants.
        AssetBundleManager.ActiveVariants = activeVariants;

        // Show the two ActiveVariants in the console.
        Debug.Log(AssetBundleManager.ActiveVariants[0]);

        // Load variant level which depends on variants.
        yield return StartCoroutine(InitializeLevelAsync(GetActiveFromDictionary(tableNames), true));
    }

    // Initialize the downloading url and AssetBundleManifest object.
    protected IEnumerator Initialize()
    {
        // Don't destroy this gameObject as we depend on it to run the loading script.
        //DontDestroyOnLoad(gameObject);

        AssetBundleManager.BaseDownloadingURL = "file:///" + Application.dataPath + "/../AssetBundles/" + AssetBundles.Utility.GetPlatformName() + "/";

        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
        var request = AssetBundleManager.Initialize();

        if (request != null)
            yield return StartCoroutine(request);
    }

    protected IEnumerator InitializeLevelAsync(string levelName, bool isAdditive)
    {
        // This is simply to get the elapsed time for this phase of AssetLoading.
        float startTime = Time.realtimeSinceStartup;

        // Load level from assetBundle.
        AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(sceneAssetBundle, levelName, isAdditive);
        if (request == null)
            yield break;

        yield return StartCoroutine(request);

		// Trigger callbacks once the scene has loaded
		//OnFinished(true);
		finishedLoading = true;

		if (NetworkManager.singleton.isNetworkActive && NetworkServer.connections.Count > 0)
		{
			NetworkServer.SpawnObjects();
		}

        // Calculate and display the elapsed time.
        float elapsedTime = Time.realtimeSinceStartup - startTime;
        Debug.Log("Finished loading scene " + levelName + " in " + elapsedTime + " seconds");
    }
    
	public void ManualLoad(string variantName, string tableName)
	{
		SetActiveInDictionary(variantName, variantNames);
		SetActiveInDictionary(tableName, tableNames);

		bundlesLoaded = true;
		// Set the activeVariant
		activeVariants[0] = "table-" + GetActiveFromDictionary(variantNames);
		// Load the scene now!
		StartCoroutine(BeginExample());
	}
		
	private string GetKeyFromDictionaryAt(int index, Dictionary<string, bool> dict)
	{
		int i = 0;

		foreach (KeyValuePair<string, bool> kvp in dict)
		{
			if (i == index) 
			{
				return kvp.Key;
			}

			i++;
		}

		Debug.Assert(true, "No key exists at that index");

		return "no key at that index";
	}

	private int GetIndexAtDictionaryKey(string key, Dictionary<string, bool> dict)
	{
		int i = 0;

		foreach (KeyValuePair<string, bool> kvp in dict)
		{
			if (kvp.Key == key) 
			{
				return i;
			}

			i++;
		}

		Debug.Assert(true, "No key exists at that index");

		return 0;
	}

	public int GetCount(string type)
	{
		if (type == "table")
		{
			return tableNames.Count;
		}
		else if (type == "variant")
		{
			return variantNames.Count;
		}
		else if (type == "paddle")
		{
			return paddleNames.Count;
		}
		else
		{
			Debug.Log("type " + type + " does not exist");
			return 0;
		}
	}

	public string GetName(string type, int index)
	{
		if (type == "table")
		{
			return GetKeyFromDictionaryAt(index, tableNames);
		}
		else if (type == "variant")
		{
			return GetKeyFromDictionaryAt(index, variantNames);
		}
		else if (type == "paddle")
		{
			return GetKeyFromDictionaryAt(index, paddleNames);
		}
		else
		{
			Debug.Log("type " + type + " does not exist");
			return "";
		}
	}

	// Load the scene using the given table, variant and paddle
	public void LoadScene(int tableIndex, int variantIndex, int paddleIndex)
	{
		// Remove the buttons
		bundlesLoaded = true;

		SetActiveInDictionary(GetName("table", tableIndex), tableNames);
		SetActiveInDictionary(GetName("variant", variantIndex), variantNames);
		SetActiveInDictionary(GetName("paddle", paddleIndex), paddleNames);

		// Set the activeVariant
		activeVariants[0] = "table-" + GetActiveFromDictionary(variantNames);

		if (NetworkManager.singleton.isNetworkActive)
		{
			GameObject.FindGameObjectWithTag("TableNetworking").GetComponent<TableNetworking>().SetTableInfo(GetActiveFromDictionary(variantNames), GetActiveFromDictionary(tableNames));
		}

		// Load the scene now!
		StartCoroutine(BeginExample());
	}

	public bool HasFinishedLoading()
	{
		return finishedLoading;
	}

	// Sets whether to play against an AI or not
	public void SetSinglePlayer(bool singlePlayer)
	{
		isSinglePlayer = singlePlayer;
	}

	public IEnumerator BeginPlaying()
	{
		GetComponent<GameMenuHandlerUGUI>().CloseUI();

		// Fill the table with AIs if offline or the server
		if (!NetworkManager.singleton.isNetworkActive || NetworkServer.connections.Count > 0)
		{
			// Add the ball spawner script to a spawner in the scene otherwise it will spawn inactive
			GameObject[] spawners = GameObject.FindGameObjectsWithTag("BallSpawner");
			Debug.Assert(spawners.Length > 0, "Need at least one spawner in the scene so we know where to put the ball");
			GameObject spawner = spawners[Random.Range(0, spawners.Length - 1)];
			spawner.AddComponent<BallSpawner>().ballPrefab = NetworkManager.singleton.spawnPrefabs[0];// Don't like this at all...
			spawner.AddComponent<ResetBallUI>();

           

            //if (GameObject.FindGameObjectsWithTag("Score").Length <= 0)
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
				//int paddleIndex = Mathf.FloorToInt(Random.Range(0, playerPrefabs.Length) );
				var paddlePrefab = playerPrefabs[GetIndexAtDictionaryKey(GetActiveFromDictionary(paddleNames), paddleNames)];
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

				playerIndex++;

				//this should change one Player scripts playerNum to 1 and one to 2 in order to allow 2 players
				// Doesn't seem to be working. atm.
				paddle.GetComponent<Player>().playerNum = playerIndex;
				//Debug.Log("paddleIndex is "+ paddleIndex);
			}
		}

		yield return new WaitForSeconds(2);

		SimpleAI[] bots = GameObject.FindObjectsOfType<SimpleAI>();

		// If local make the first paddle player controlled
		if (!NetworkManager.singleton.isNetworkActive)
		{
			bots[1].gameObject.GetComponent<Player>().enabled = true;
			bots[1].enabled = false;
			bots[1].gameObject.GetComponent<Remote>().enabled = false;

			if (isSinglePlayer)
			{
				// Set player 2 to be a bot if singleplayer
				bots[0].gameObject.GetComponent<Player>().enabled = false;
				bots[0].enabled = true;
				bots[0].gameObject.GetComponent<Remote>().enabled = false;
			}
			else
			{
				// Set player 2 to be a player if not singleplayer
				bots[0].gameObject.GetComponent<Player>().enabled = true;
				bots[0].enabled = false;
				bots[0].gameObject.GetComponent<Remote>().enabled = false;
			}
		}
		// if networked make the host control paddle 0 and the client control paddle 1
		else
		{
			if (NetworkServer.connections.Count > 0)
			{
				bots[0].gameObject.GetComponent<PaddleNetworking>().PossessPaddle();
				bots[0].gameObject.GetComponent<Player>().enabled = true;
			}
			else
			{
				bots[1].gameObject.GetComponent<PaddleNetworking>().PossessPaddle();
				bots[1].gameObject.GetComponent<Player>().enabled = true;
			}
		}

        //Create the power UI for the players
        const string powerUIPrefab = "entities/powers/PowerUI";
        Instantiate(Resources.Load(powerUIPrefab));
    }
}
