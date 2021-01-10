using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class CD : Item
{
    private Quest questFindCd;
    private Quest questLibraryBook;

    protected override void Start()
    {
        base.Start();
        itemName = "CD";

        //questFindCd = questManager.FindQuest("Parsley Find CD");
        //questLibraryBook = questManager.FindQuest("Oakewood Library Book");
    }

    protected override bool CheckForPickup()
    {
        return (questFindCd.IsInProgress() || questLibraryBook.IsInProgress());
    }
}
