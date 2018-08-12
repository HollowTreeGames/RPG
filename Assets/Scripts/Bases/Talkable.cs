using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDialogue;

public class Talkable : Interactable {
    
    public DLine[] dialogue;

    protected DialogueManager dialogueManager;
    
    protected override void Start()
    {
        base.Start();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public override void Interact()
    {
        dialogueManager.StartDialogue(GetDialogue());
    }

    protected virtual DLine[] GetDialogue()
    {
        return dialogue;
    }
}
