using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCanvas : MonoBehaviour {

    private static bool instanceExists = false;
    
    private void Awake () {
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
