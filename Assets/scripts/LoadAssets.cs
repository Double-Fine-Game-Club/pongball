using AssetBundles;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

public class LoadAssets : MonoBehaviour {
    public string sceneAssetBundle;

    // TODO: Can we display dictionaries in the editor??
    public Dictionary<string, bool> variantNames = new Dictionary<string, bool>() {
        { "lightsoda", false },
        { "kgunn", false }
    };
    public Dictionary<string, bool> tableNames = new Dictionary<string, bool>() {
        { "Table_Clean", false },
        { "Table01", false },
        { "Table02", false },
        { "Speedster", false }
    };

    private string[] activeVariants;
	private bool finishedLoading;

	public bool bundlesLoaded;

	public delegate void FinishedCallback(bool success);
	public FinishedCallback OnFinished;

    void Awake()
    {
        activeVariants = new string[2];
        bundlesLoaded = false;
		finishedLoading = false;

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
        DontDestroyOnLoad(gameObject);

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
		OnFinished(true);
		finishedLoading = true;

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

	public int GetVariantCount()
	{
		return variantNames.Count;
	}

	public int GetTableCount()
	{
		return tableNames.Count;
	}

	public string GetVariantName(int variantIndex)
	{
		return GetKeyFromDictionaryAt(variantIndex, variantNames);
	}

	public string GetTableName(int tableIndex)
	{
		return GetKeyFromDictionaryAt(tableIndex, tableNames);
	}

	public void LoadScene(int variantIndex, int tableIndex)
	{
		// Remove the buttons
		bundlesLoaded = true;

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
}
