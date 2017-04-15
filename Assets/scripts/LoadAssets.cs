using AssetBundles;
using System.Collections;
using UnityEngine;
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
        { "Table01", false },
        { "Table02", false }
    };

    private string[] activeVariants;

    private bool bundlesLoaded;

    void Awake()
    {
        activeVariants = new string[2];
        bundlesLoaded = false;

        Debug.Assert(variantNames != null, "No variant names assigned");
        Debug.Assert(tableNames != null, "No table names assigned");
    }

    // Creating the Temp UI for the demo in IMGui.
    void OnGUI()
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
                // Load the scene now!
                StartCoroutine(BeginExample());
            }

            // End GUI Padding
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }

    private void AddGUIButtons()
    {
        GUILayout.BeginVertical();
        foreach(KeyValuePair<string,bool> variant in variantNames)
        {
            GUILayout.Toggle(variant.Value, "");
            if(GUILayout.Button(variant.Key))
            {
                SetActiveInDictionary(variant.Key, variantNames);
            }
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        foreach (KeyValuePair<string, bool> table in tableNames)
        {
            GUILayout.Toggle(table.Value, "");
            if (GUILayout.Button(table.Key))
            {
                SetActiveInDictionary(table.Key, tableNames);
            }
        }
        GUILayout.EndVertical();
    }

    private void SetActiveInDictionary(string activeKey, Dictionary<string, bool> dict)
    {
        foreach (string key in new List<string>(dict.Keys))
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

        AssetBundleManager.BaseDownloadingURL = "file:///" + Application.dataPath + "/../AssetBundles/" + Utility.GetPlatformName() + "/";

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

        // Calculate and display the elapsed time.
        float elapsedTime = Time.realtimeSinceStartup - startTime;
        Debug.Log("Finished loading scene " + levelName + " in " + elapsedTime + " seconds");
    }
    
}
