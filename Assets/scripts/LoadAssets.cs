using AssetBundles;
using System.Collections;
using UnityEngine;
using System;

public class LoadAssets : MonoBehaviour {
    public string sceneAssetBundle;
    public string sceneName;

    private string[] activeVariants;

    private bool bundlesLoaded;
    private bool normal, alternate;
    private string tableStyle;

    void Awake()
    {
        activeVariants = new string[2];
        bundlesLoaded = false;
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

            // GUI Buttons
            // New Line - Get HD/SD
            GUILayout.BeginHorizontal();
            // Display the choice
            GUILayout.Toggle(normal, "");
            // Get player choice
            if (GUILayout.Button("Normal Table")) { normal = true; alternate = false; tableStyle = "normal"; }
            // Display the choice
            GUILayout.Toggle(alternate, "");
            // Get player choice
            if (GUILayout.Button("Alternate Table")) { normal = false; alternate = true; tableStyle = "alternate"; }
            GUILayout.EndHorizontal();

            // GUI Padding
            GUILayout.Space(15);

            // Load the Scene
            if (GUILayout.Button("Load Scene"))
            {
                // Remove the buttons
                bundlesLoaded = true;
                // Set the activeVariant
                activeVariants[0] = "table-" + tableStyle;
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

    // Use this for initialization
    IEnumerator BeginExample()
    {
        yield return StartCoroutine(Initialize());

        // Set active variants.
        AssetBundleManager.ActiveVariants = activeVariants;

        // Show the two ActiveVariants in the console.
        Debug.Log(AssetBundleManager.ActiveVariants[0]);

        // Load variant level which depends on variants.
        yield return StartCoroutine(InitializeLevelAsync(sceneName, true));
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
