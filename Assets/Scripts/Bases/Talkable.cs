using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDialogue;
using Yarn.Unity;

public class Talkable : Interactable {
    
    protected DialogueRunner dialogueRunner;

    [Header("Optional")]
    public TextAsset scriptToLoad;

    protected override void Start()
    {
        base.Start();
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        if (scriptToLoad != null)
        {
            dialogueRunner.AddScript(scriptToLoad);
        }
        else
        {
            //Debug.LogWarning(Utils.Join(name, "has no Yarn script loaded!"));
        }
    }

    public override void Interact()
    {
        dialogueRunner.StartDialogue(this.name + " Start");
    }
}
