using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultButtonSelector : MonoBehaviour {

	public Selectable defaultWidget;

	// Use this for initialization
	void Start () {
		defaultWidget.Select();
	}
}
