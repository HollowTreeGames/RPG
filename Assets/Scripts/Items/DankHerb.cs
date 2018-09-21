using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class DankHerb : Item
{
    private Quest questDankHerb;
    private Quest questLibraryBook;
    private Quest questFindCd;

    protected override void Start()
    {
        base.Start();
        itemName = "Dank Herb";

        questDankHerb = questManager.FindQuest("Henry Dank Herb");
        questLibraryBook = questManager.FindQuest("Oakewood Library Book");
        questFindCd = questManager.FindQuest("Parsley Find CD");
    }

    protected override bool CheckForPickup()
    {
        return (questDankHerb.IsInProgress() || questFindCd.IsInProgress() || questLibraryBook.IsInProgress());
    }
}
