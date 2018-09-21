using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class LibraryBook : Item
{
    private Quest questLibraryBook;

    protected override void Start()
    {
        base.Start();
        itemName = "Library Book";

        questLibraryBook = questManager.FindQuest("Oakewood Library Book");
}

    protected override bool CheckForPickup()
    {
        return questLibraryBook.IsInProgress();
    }
}
