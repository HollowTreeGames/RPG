using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CinematicEngine : MonoBehaviour {

    public TextAsset scriptToLoad;

    private DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    private void Start() {
        dialogueRunner.AddScript(scriptToLoad);
        dialogueRunner.StartDialogue("Cinematic Start");
    }
}
