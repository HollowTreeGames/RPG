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

        questDankBook = questManager.FindQuest("henryDankBook");
        questLibraryBook = questManager.FindQuest("oakewoodLibraryBook");
    }

    protected override bool CheckForPickup()
    {
        return (questDankBook.IsInProgress() || questLibraryBook.IsInProgress());
    }
}
