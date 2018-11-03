using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : MonoBehaviour {

    private static bool instanceExists = false;
    
    void Start () {
        if (instanceExists)
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(gameObject);
    }
}
