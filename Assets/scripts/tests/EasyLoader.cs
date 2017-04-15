using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasyLoader : MonoBehaviour {


	void Start () {
        DontDestroyOnLoad(gameObject);
		//Asset Bundle Manager confuses me, so this is just going to load the scene so I can test it.  Might not work.
        SceneManager.LoadSceneAsync("Table02");
	}
	

}
