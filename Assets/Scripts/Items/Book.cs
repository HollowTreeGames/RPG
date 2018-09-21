using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class Book : Item
{
    private Quest questDankBook;
    private Quest questLibraryBook;

    protected override void Start()
    {
        base.Start();
        itemName = "Herb Book";

        questDankBook = questManager.FindQuest("Henry Dank Book");
        questLibraryBook = questManager.FindQuest("Oakewood Library Book");
    }

    protected override bool CheckForPickup()
    {
        return (questDankBook.IsInProgress() || questLibraryBook.IsInProgress());
    }
}
