using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DankHerb : Item
{
    private GameState gameState;
    private DialogueManager dialogueManager;

    protected override void Start()
    {
        base.Start();
        itemName = "Dank Herb";
        gameState = FindObjectOfType<GameState>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public override void Interact()
    {
        if (gameState.findTheDankHerb)
        {
            base.Interact();
        }
        else
        {
            dialogueManager.StartDialogue("", "Mmmmm! That smells like some straight up bodacious bud!");
        }
    }
}
