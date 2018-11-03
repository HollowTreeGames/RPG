﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuePrompt : MonoBehaviour {

    private static bool instanceExists = false;
    
    void Start () {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(gameObject);
    }
}
