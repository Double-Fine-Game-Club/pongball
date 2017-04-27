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
        { "table", 3 },
        { "variant", 1 },
        { "paddle", 4 },
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
        if (currentIndex[selectionType] > 0)
        {
            currentIndex[selectionType]--;
        }
        else
        {
            currentIndex[selectionType] = indexLength;
        }
        SetSelectionText(selectionType, loadAssets.GetName(selectionType, currentIndex[selectionType]));
    }

    public void LoadGameCoroutine()
    {
        StartCoroutine( LoadGame());
    }

    public IEnumerator LoadGame()
    {
        //GetComponent.makeSelectionText.text
        //LoadScene
        loadAssets.LoadScene(currentIndex["table"], currentIndex["variant"], currentIndex["paddle"]);
        
        while (loadAssets.HasFinishedLoading() != true)
        {
                  yield return null;
        }

		StartCoroutine(loadAssets.BeginPlaying());
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

