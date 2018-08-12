using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class CD : Item {

    protected override void Start()
    {
        base.Start();
        itemName = "CD";
        gameState = FindObjectOfType<GameState>();

        DLine[] pickUp =
        {
            new DLine("Belfry", "Default", "Ooo, shiny!"),
            new DLine("Belfry", "Happy", "'America Online Installation CD.'"),
            new DLine("Belfry", "Default", "The colors are so pretty on this..."),
            new DLine("Belfry", "Happy", "This seems cool!")
        };
        this.pickUp = pickUp;

        DLine[] handsFull =
        {
            new DLine("Belfry", "Sad", "That looks delicate. I probably need both paws to hold it!")
        };
        this.handsFull = handsFull;

        DLine[] defaultDialogue =
        {
            new DLine("Belfry", "Happy", "It's a shiny...circle...disk thingy!!")
        };
        this.defaultDialogue = defaultDialogue;
    }

    protected override bool CheckForPickup()
    {
        return (gameState.findCD == QuestState.InProgress);
    }
}
