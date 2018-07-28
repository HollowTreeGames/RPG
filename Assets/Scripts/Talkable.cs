using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : Interactable {

    public string charName;
    public string[] dialogue;

    public override void Interact()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(charName, GetDialogue());
    }

    protected virtual string[] GetDialogue()
    {
        return dialogue;
    }
}
