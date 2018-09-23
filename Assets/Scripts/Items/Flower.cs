using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using MyDialogue;

public class Flower : Item
{
    DLine[] myPickUp =
    {
        new DLine("Belfry", "Default", "I could smell this from where I spawned in."),
        new DLine("Belfry", "Default", "I think that qualifies this as the dankest Mary Jane."),
        new DLine("Belfry", "Sad", "Hopefully Henry will like it, even though his wife's name is Nora."),
    };
    DLine[] myHandsFull =
    {
        new DLine("Belfry", "Sad", "That looks sublimely sticky with sweet dew. I don't want to get any of it on the other thing I'm carrying.")
    };
    DLine[] myDefaultDialogue =
    {
        new DLine("Belfry", "Happy", "Mmmmm! That smells like some straight up bodacious bud!")
    };

    private Quest questGetFlower;

    protected override void Start()
    {
        base.Start();
        itemName = "Flower";

        this.pickUp = myPickUp;
        this.handsFull = myHandsFull;
        this.defaultDialogue = myDefaultDialogue;

        questGetFlower = questManager.FindQuest("belfryGetFlower");
    }

    protected override bool CheckForPickup()
    {
        return questGetFlower.IsInProgress();
    }
}
