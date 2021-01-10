using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class LibraryBook : Item
{
    private Quest questLibraryBook;
    private Quest questFindCd;

    protected override void Start()
    {
        base.Start();
        itemName = "Library Book";

        //questLibraryBook = questManager.FindQuest("Oakewood Library Book");
        //questFindCd = questManager.FindQuest("Parsley Find CD");
}

    protected override bool CheckForPickup()
    {
        return (questLibraryBook.IsInProgress() || questFindCd.IsInProgress());
    }
}
