using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class CreditsParser : MonoBehaviour {

	private float posInitial;
	private float posMax;
	private int scrollSpeed = 20;

	// Use this for initialization
	void Start ()
	{
		Text textElement = GetComponent<Text>();
		textElement.text = "Pongball has been made by the following community contributors\n(see the Pongball GitHub repository for licence notices):\n\n" + GetCreditsNames() + "\n\n\n"
						+ "Pongball makes use of the following third party libraries/code:\n\n"
						+ "Asset Bundle Manager - Unity Technologies\n"
						+ "SoundManager.cs - Unity Technologies\n";
						//TODO: Add details/licences of any other third party code?
		posInitial = - textElement.preferredHeight / 2;
		posMax = Screen.height + textElement.preferredHeight / 2;
		var t = transform.position;
		t.y = posInitial;
		transform.position = t;
	}

	// Update is called once per frame
	void Update()
	{
		var t = transform.position;
		if (t.y >= posMax)
		{
			t.y = posInitial;
		}
		else
		{
			t.y += scrollSpeed * Time.deltaTime;
		}
		transform.position = t;
	}

	string GetCreditsNames()
	{
		//Load the names from the main repo's git contributors and split it into a list based on newlines
		TextAsset ta = Resources.Load<TextAsset>("credits/credits-git");
		List<string> namesList = ta.text.Replace("\r\n", "\n").Replace("\r","\n").Split("\n"[0]).ToList();

		//Load the names from the assets repo's git contributors and split it into a list based on newlines
		ta = Resources.Load<TextAsset>("credits/credits-assets-git");
		var assetsNames = ta.text.Replace("\r\n", "\n").Replace("\r","\n").Split("\n"[0]);

		//Loop through and any names in the assets repo list that aren't already present
		foreach (string s in assetsNames)
		{
			if (!namesList.Contains(s))
			{
				namesList.Add(s);
			}
		}


		//Load the names/details from the credits-extra file and split it into a list based on newlines
		ta = Resources.Load<TextAsset>("credits/credits-extra");
		List<string> extra = ta.text.Replace("\r\n", "\n").Replace("\r","\n").Split("\n"[0]).ToList();

		//Loop throught and find lines that aren't already present
		foreach (string s in extra)
		{
			if (!namesList.Contains(s))
			{
				//Loop through looking for matches in namesList and update that entry if we find one
				int i = s.Length - 1;
				while (i > 0)
				{
					string tempString = s.Substring(0, i);
					if (namesList.Contains(tempString))
					{
						namesList[namesList.IndexOf(tempString)] = s;
						break;
					}
					else
					{
						i = s.LastIndexOf(" "[0], i-1);
					}
				}

				//If we couldn't find a match, the name is new, so add it
				if (i < 0)
				{
					namesList.Add(s);
				}
			}
		}

		namesList.Sort();
		while (namesList.Contains(""))
		{
			namesList.Remove("");
		}
		while (namesList.Contains("Cheese"))
		{
			namesList.Remove("Cheese");
		}

		//Turn it all back into one string and return it
		return String.Join("\n", namesList.ToArray());
	}
}
