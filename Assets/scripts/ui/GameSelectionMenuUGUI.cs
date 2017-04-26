using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelectionMenuUGUI : MonoBehaviour {
    private LoadAssets loadAssets;
    public Text tableSelectionText;
    public Text variantSelectionText;
    public Text paddleSelectionText;
    //private int currentTable;
    //public string selectionType;
    private int indexLength;

    public Dictionary<string, int> currentIndex = new Dictionary<string, int>() {
        { "table", 0 },
        { "variant", 0 },
        { "paddle", 0 },
    };



    void Start()
    {
        loadAssets = GetComponent<LoadAssets>();
        foreach (KeyValuePair<string, int> selectionType in currentIndex)
        {
            SetSelectionText(selectionType.Key, loadAssets.GetName(selectionType.Key, currentIndex[selectionType.Key]));

        }


    }


    public void ForwardButton(string selectionType)
    {
        indexLength = loadAssets.GetCount(selectionType) - 1;
        if (currentIndex[selectionType] < indexLength)
        {
            currentIndex[selectionType]++;
        }
        else
        {
            currentIndex[selectionType] = 0;
        }
        SetSelectionText(selectionType, loadAssets.GetName(selectionType, currentIndex[selectionType]));

    }

    public void BackButton(string selectionType)
    {
        indexLength = loadAssets.GetCount(selectionType) - 1;
        if (currentIndex[selectionType] < indexLength)
        {
            currentIndex[selectionType] = indexLength;
        }
        else
        {
            currentIndex[selectionType]--;
        }
        SetSelectionText(selectionType, loadAssets.GetName(selectionType, currentIndex[selectionType]));
    }

    public void LoadGameCoroutine()
    {
        StartCoroutine( LoadGame());
    }

    public IEnumerator LoadGame()
    {
        Debug.Log("Stuff is sort of working!!!!");
        //GetComponent.makeSelectionText.text
        //LoadScene
        loadAssets.LoadScene(currentIndex["table"], currentIndex["variant"], currentIndex["paddle"]);
        Debug.Log("fffffffff");

        while (loadAssets.HasFinishedLoading() != true)
        {
            Debug.Log("uuuuuu");
                  yield return null;
            Debug.Log("HasFinishedLoading equals" + loadAssets.HasFinishedLoading());
        }
        Debug.Log("fuck yeah");
        loadAssets.BeginPlaying();
    }

    private void SetSelectionText(string selectionType, string selectionText)
    {
        switch (selectionType)
        {  
            case "table" : 
                tableSelectionText.GetComponent<Text>().text = selectionText;

                break;
            case "variant":
                variantSelectionText.GetComponent<Text>().text = selectionText;
                break;
            case "paddle":
                paddleSelectionText.GetComponent<Text>().text = selectionText;
                break;
            default:
                Debug.Log("Something went wrong with selectionType");
                break;
        }
    }




}

