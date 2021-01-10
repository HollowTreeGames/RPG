using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class DankBook : Item
{
    private Quest questDankBook;
    private Quest questLibraryBook;
    private Quest questFindCd;

    protected override void Start()
    {
        base.Start();
        itemName = "Herb Book";

        //questDankBook = questManager.FindQuest("Henry Dank Book");
        //questFindCd = questManager.FindQuest("Parsley Find CD");
        //questLibraryBook = questManager.FindQuest("Oakewood Library Book");
    }

    protected override bool CheckForPickup()
    {
        return (questDankBook.IsInProgress() || questFindCd.IsInProgress() || questLibraryBook.IsInProgress());
    }
}
