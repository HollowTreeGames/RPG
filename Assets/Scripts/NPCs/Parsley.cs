using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Enums;
using MyDialogue;

public class Parsley : NPC {
    
    private DLine[] initialDialogue =
    {
        new DLine("Parsley", "Default", "Hmph.")
    };

    private DLine[] friendlyDialogue =
    {
        new DLine("Parsley", "Sad", "I guess you didn't MEAN to burn down the whole town..."),
        new DLine("Parsley", "Default", "Fine, we can talk again. But don't think I'm ever loaning you another candle!")
    };

    private DLine[] questCDDialogue =
    {
        new DLine("Parsley", "Default", "Oh alright, alright!!"),
        new DLine("Parsley", "Default", "I'll give you a chance to redeem yourself."),
        new DLine("Parsley", "Happy", "Find the coolest possible thing you can. If it's cool enough, I might loan you another candle."),
        new DLine("Parsley", "Default", "UNLIT, OF COURSE.")
    };

    private DLine[] questBookCDDialogue =
    {
        new DLine("Parsley", "Default", "I said bring the COOLEST possible thing."),
        new DLine("Parsley", "Sad", "Books are cool, but this one is lame.")
    };

    private DLine[] questHerbCDDialogue =
    {
        new DLine("Parsley", "Default", "That is literally marijuana."),
        new DLine("Parsley", "Default", "I'm ten."),
        new DLine("Parsley", "Default", "You're not supposed to give ten year olds marijuana."),
        new DLine("Parsley", "Default", "Find something better.")
    };

    private DLine[] itemCDDialogue =
    {
        new DLine("Parsley", "Happy", "Whoa!!"),
        new DLine("Parsley", "Happy", "I've been looking for one of these!"),
        new DLine("Parsley", "Sad", "Hmm... it's no Nickelback, but I'll take it."),
        new DLine("Parsley", "Happy", "Thanks, Bel! You're the best!")
    };

    private DLine[] thanksDialogue =
    {
        new DLine("Parsley", "Happy", "Have you heard these mad beats?"),
        new DLine("Parsley", "Happy", "They sound like nothing I've ever heard before!"),
        new DLine("Parsley", "Happy", "Here, listen!"),
        new DLine("Belfry", "Sad", "<You hear the sound of a modem trying to start up.>")
    };

    private DLine[] reminderDialogue =
    {
        new DLine("Parsley", "Happy", "Have you found the COOLEST THING EVER yet?"),
        new DLine("Parsley", "Happy", "Please be sure it is quantifiably the coolest thing you've ever set paw upon.")
    };

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateQuests()
    {
        if (gameState.reputation >= 3 && gameState.findCD == QuestState.Unavailable)
        {
            gameState.findCD = QuestState.Available;
        }
    }

    protected override bool IsQuestAvailable()
    {
        return (gameState.findCD == QuestState.Available);
    }

    protected override DLine[] GetDialogue()
    {
        if (gameState.reputation < 2)
        {
            return initialDialogue;
        }

        switch (gameState.findCD)
        {
            case QuestState.Available:
                gameState.findCD = QuestState.InProgress;
                return questCDDialogue;
            case QuestState.InProgress:
                if (inventoryManager.GetInventory() == "CD")
                {
                    gameState.reputation += 1;
                    gameState.friendshipParsley += 1;
                    inventoryManager.ClearInventory();
                    gameState.findCD = QuestState.Done;
                    return itemCDDialogue;
                }
                else if (inventoryManager.GetInventory() == "Library Book" || inventoryManager.GetInventory() == "Herb Book")
                {
                    inventoryManager.ClearInventory();
                    return questBookCDDialogue;
                }
                else if (inventoryManager.GetInventory() == "Dank Herb")
                {
                    inventoryManager.ClearInventory();
                    return questHerbCDDialogue;
                }
                else
                {
                    return reminderDialogue;
                }
            case QuestState.Done:
                return thanksDialogue;

            default:
                break;
        }

        return friendlyDialogue;
    }
}
