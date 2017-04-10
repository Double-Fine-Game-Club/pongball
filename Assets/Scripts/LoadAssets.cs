using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAssets : MonoBehaviour {
	void Start () {
        SceneManager.LoadSceneAsync("Table01", LoadSceneMode.Additive);
	}
}
