using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class CD : Item
{
    private Quest questFindCd;

    protected override void Start()
    {
        base.Start();
        itemName = "CD";

        questFindCd = questManager.FindQuest("Parsley Find CD");
}

    protected override bool CheckForPickup()
    {
        return questFindCd.IsInProgress();
    }
}
