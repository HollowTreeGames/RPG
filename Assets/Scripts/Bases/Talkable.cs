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
    }

    public override void Interact()
    {
        dialogueRunner.StartDialogue(GetDialogueNode());
    }

    protected virtual string GetDialogueNode()
    {
        return "Start";
    }
}
