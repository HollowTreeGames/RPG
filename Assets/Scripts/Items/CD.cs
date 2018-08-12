using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class CD : Item {

    private GameState gameState;

    protected override void Start()
    {
        base.Start();
        itemName = "CD";
        gameState = FindObjectOfType<GameState>();
    }

    public override void Interact()
    {
        if (gameState.findCD == QuestState.InProgress)
        {
            base.Interact();
        }
        else
        {
            dialogueManager.StartDialogue(new DLine("Belfry", "Default", 
                "America Online Installation CD-ROM. It's sure shiny! I like the way it catches the light!"
                ));
        }
    }
}
