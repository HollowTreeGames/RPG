using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class LibraryBook : Item
{
    DLine[] myPickUp =
    {
        new DLine("Belfry", "Sad", "What a respectable, hefty tome!"),
        new DLine("Belfry", "Default", "Let's see..."),
        new DLine("Belfry", "Sad", "'Fur-Couture: How to Knit With Your Own Sheddings.'"),
        new DLine("Belfry", "Sad", "...maybe Oakewood won't look too closely at the title.")
    };
    DLine[] myHandsFull =
    {
        new DLine("Belfry", "Sad", "After what happened to my last library book, I should probably wait until I can carry this with two paws.")
    };
    DLine[] myDefaultDialogue =
    {
        new DLine("Belfry", "Happy", "It's an old book! What's it doing under a tree in the middle of a forest?")
    };

    private Quest questLibraryBook;

    protected override void Start()
    {
        base.Start();
        itemName = "Library Book";

        this.pickUp = myPickUp;
        this.handsFull = myHandsFull;
        this.defaultDialogue = myDefaultDialogue;

        questLibraryBook = questManager.FindQuest("oakewoodLibraryBook");
}

    protected override bool CheckForPickup()
    {
        return questLibraryBook.IsInProgress();
    }
}
