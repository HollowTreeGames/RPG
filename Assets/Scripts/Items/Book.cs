﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class Book : Item
{
    DLine[] myPickUp =
    {
        new DLine("Belfry", "Default", "What's this?"),
        new DLine("Belfry", "Sad", "Hashish and You: An Illustrated Guide to the Mile High Club"),
        new DLine("Belfry", "Sad", "..."),
        new DLine("Belfry", "Happy", "Sounds right to me!")
    };
    DLine[] myHandsFull =
    {
        new DLine("Belfry", "Sad", "A book this heavy with knowledge needs two hands to carry.")
    };
    DLine[] myDefaultDialogue =
    {
        new DLine("Belfry", "Sad", "This is one of those ... illicit books. ABOUT DRUGS!")
    };

    private Quest questDankBook;
    private Quest questLibraryBook;

    protected override void Start()
    {
        base.Start();
        itemName = "Herb Book";

        this.pickUp = myPickUp;
        this.handsFull = myHandsFull;
        this.defaultDialogue = myDefaultDialogue;

        questDankBook = questManager.FindQuest("henryDankBook");
        questLibraryBook = questManager.FindQuest("oakewoodLibraryBook");
}

    protected override bool CheckForPickup()
    {
        return (questDankBook.IsInProgress() || questLibraryBook.IsInProgress());
    }
}
