using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class DankHerb : Item
{
    private GameState gameState;

    protected override void Start()
    {
        base.Start();
        itemName = "Dank Herb";
        gameState = FindObjectOfType<GameState>();
    }

    public override void Interact()
    {
        if (gameState.findTheDankHerb == QuestState.InProgress)
        {
            base.Interact();
        }
        else
        {
            dialogueManager.StartDialogue("", "Mmmmm! That smells like some straight up bodacious bud!");
        }
    }
}
