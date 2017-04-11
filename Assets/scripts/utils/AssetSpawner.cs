using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSpawner : MonoBehaviour {

    public GameObject[] playingFieldPrefabs;
    public GameObject bumperPrefab;
    public Mesh[] bumperMeshes;

    private void Start()
    {
        for(int index = 0; index < transform.childCount; index++)
        {
            var child = transform.GetChild(index);
            
            switch(child.name)
            {
                case "Playing Field":
                    AddPlayingField(child);
                    break;
                case "Bumpers":
                    AddBumpers(child);
                    break;
            }
        }
    }

    private void AddBumpers(Transform child)
    {
        if (playingFieldPrefabs.Length <= 0)
        {
            Debug.Log("No bumper prefabs to add");
            return;
        }

        // TODO: We assume child is a set of bumper transforms
        for (int index = 0; index < child.childCount; index++)
        {
            var bumperTransform = child.GetChild(index);
            GameObject bumper = Instantiate(
                bumperPrefab,
                bumperTransform.position,
                bumperTransform.rotation
                );
            bumper.GetComponent<MeshFilter>().mesh = bumperMeshes[UnityEngine.Random.Range(0, bumperMeshes.Length)];
        }
    }

    private void AddPlayingField(Transform child)
    {
        if(playingFieldPrefabs.Length <= 0)
        {
            Debug.Log("No playing field prefabs to add from");
            return;
        }

        Instantiate(
            playingFieldPrefabs[UnityEngine.Random.Range(0, playingFieldPrefabs.Length)],
            child.position,
            child.rotation
            );
    }
}
