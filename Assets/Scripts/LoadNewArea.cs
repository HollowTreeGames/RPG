using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNewArea : MonoBehaviour {
    
    public string levelToLoad;
    public float startX;
    public float startY;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        if (otherObject.name == "Player") {
            SceneManager.LoadScene(levelToLoad);
            otherObject.transform.position = new Vector2(startX, startY);
        }
    }
}
