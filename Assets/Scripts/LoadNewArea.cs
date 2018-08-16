using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNewArea : MonoBehaviour {
    
    public string levelToLoad;
    public float startX;
    public float startY;

    private FadeCameraAndLoad fadeCamera;

    private void Start()
    {
        fadeCamera = FindObjectOfType<FadeCameraAndLoad>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        if (otherObject.name == "Player") {
            fadeCamera.StartLoad(otherObject, levelToLoad, startX, startY);
        }
    }

    
}
