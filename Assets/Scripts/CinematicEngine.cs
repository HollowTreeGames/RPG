using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CinematicEngine : MonoBehaviour {

    bool cinematicStarted;
    DialogueRunner dialogueRunner;

    private void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!cinematicStarted)
        {
            cinematicStarted = true;
            dialogueRunner.StartDialogue("Cinematic Start");
        }
    }
}
