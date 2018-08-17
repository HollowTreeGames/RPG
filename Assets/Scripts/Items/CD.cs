using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class CD : Item
{
    DLine[] myPickUp =
    {
        new DLine("Belfry", "Default", "Ooo, shiny!"),
        new DLine("Belfry", "Happy", "'America Online Installation CD.'"),
        new DLine("Belfry", "Default", "The colors are so pretty on this..."),
        new DLine("Belfry", "Happy", "This seems cool!")
    };
    DLine[] myHandsFull =
    {
        new DLine("Belfry", "Sad", "That looks delicate. I probably need both paws to hold it!")
    };
    DLine[] myDefaultDialogue =
    {
        new DLine("Belfry", "Happy", "It's a shiny...circle...disk thingy!!")
    };

    private Quest questFindCd;

    protected override void Start()
    {
        base.Start();
        itemName = "CD";

        this.pickUp = myPickUp;
        this.handsFull = myHandsFull;
        this.defaultDialogue = myDefaultDialogue;

        questFindCd = questManager.FindQuest("parsleyFindCD");
}

    protected override bool CheckForPickup()
    {
        return questFindCd.IsInProgress();
    }
}
