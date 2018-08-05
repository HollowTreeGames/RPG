using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Book : Item
{
    private GameState gameState;

    protected override void Start()
    {
        base.Start();
        itemName = "Herb Book";
        gameState = FindObjectOfType<GameState>();
    }

    public override void Interact()
    {
        if (gameState.findHerbBook == QuestState.InProgress)
        {
            base.Interact();
        }
        else
        {
            dialogueManager.StartDialogue("", "This is one of those ... illicit books. ABOUT DRUGS!");
        }
    }
}
