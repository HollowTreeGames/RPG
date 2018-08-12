using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class Book : Item
{
    protected override void Start()
    {
        base.Start();
        itemName = "Herb Book";

        DLine[] pickUp = 
        {
            new DLine("Belfry", "Default", "What's this?"),
            new DLine("Belfry", "Sad", "Hashish and You: An Illustrated Guide to the Mile High Club"),
            new DLine("Belfry", "Sad", "..."),
            new DLine("Belfry", "Happy", "Sounds right to me!")
        };
        this.pickUp = pickUp;

        DLine[] handsFull =
        {
            new DLine("Belfry", "Sad", "A book this heavy with knowledge needs two hands to carry.")
        };
        this.handsFull = handsFull;

        DLine[] defaultDialogue =
        {
            new DLine("Belfry", "Sad", "This is one of those ... illicit books. ABOUT DRUGS!")
        };
        this.defaultDialogue = defaultDialogue;
    }

    protected override bool CheckForPickup()
    {
        return (gameState.findHerbBook == QuestState.InProgress || gameState.findLibraryBook == QuestState.InProgress);
    }
}
