using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNewArea : MonoBehaviour {
    
    public string levelToLoad;
    public float startX;
    public float startY;

    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        if (otherObject.name == "Player") {
            sceneLoader.StartLoad(levelToLoad, startX, startY);
        }
    }

    
}
