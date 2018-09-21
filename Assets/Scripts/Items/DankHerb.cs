using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class DankHerb : Item
{
    private Quest questDankHerb;

    protected override void Start()
    {
        base.Start();
        itemName = "Dank Herb";

        questDankHerb = questManager.FindQuest("Henry Dank Herb");
}

    protected override bool CheckForPickup()
    {
        return questDankHerb.IsInProgress();
    }
}
