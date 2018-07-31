using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : Interactable {

    public string charName;
    public string[] dialogue;

    protected DialogueManager dialogueManager;
    
    protected override void Start()
    {
        base.Start();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public override void Interact()
    {
        dialogueManager.StartDialogue(charName, GetDialogue());
    }

    protected virtual string[] GetDialogue()
    {
        return dialogue;
    }
}
