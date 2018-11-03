using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCanvas : MonoBehaviour {

    private static bool instanceExists = false;

    private void Awake()
    {
        if (instanceExists)
        {
            Destroy(gameObject);
            return;
        }

        instanceExists = true;
        DontDestroyOnLoad(gameObject);
    }
}
