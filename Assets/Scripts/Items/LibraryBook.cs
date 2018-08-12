using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class LibraryBook : Item {

    private GameState gameState;

    protected override void Start()
    {
        base.Start();
        itemName = "Library Book";
        gameState = FindObjectOfType<GameState>();
    }

    public override void Interact()
    {
        if (gameState.findLibraryBook == QuestState.InProgress)
        {
            base.Interact();
        }
        else
        {
            dialogueManager.StartDialogue(
                new DLine("Belfry", "Default", "Fur Couture: How to Knit With Your Own Sheddings. Knitting's a healthy pastime, right?")
                );
        }
    }
}
