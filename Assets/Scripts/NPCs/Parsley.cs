using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Enums;

public class Parsley : NPC {
    
    private GameState gameState;
    private InventoryManager inventoryManager;

    public Sprite CD;

    private string[] initialDialogue =
    {
        "Hmph."
    };
    private string[] friendlyDialogue =
    {
        "I guess you didn't MEAN to burn down the whole town...", 
        "Fine, we can talk again. But don't think I'm ever loaning you another candle!"
    };

    private string[] questCDDialogue =
    {
        "Alright, alright!!",
        "I'll give you a chance to redeem yourself.",
        "Find the coolest possible thing you can. If it's cool enough, I might loan you another candle.",
        "UNLIT, OF COURSE."
    };

    private string[] questBookCDDialogue =
    {
        "I said bring the COOLEST possible thing.",
        "Books are cool, but this one is lame."
    };

    private string[] questHerbCDDialogue =
    {
        "That is literally marijuana.",
        "I'm ten.",
        "You're not supposed to give ten year olds marijuana.",
        "Find something better."
    };

    private string[] itemCDDialogue =
    {
        "Whoa!!",
        "I've been looking for one of these!",
        "Hmm... it's no Nickelback, but I'll take it.",
        "Thanks, Bel! You're the best!"
    };

    private string[] thanksDialogue =
    {
        "Have you heard these mad beats?",
        "They sound like nothing I've ever heard before!",
        "Here, listen!",
        "<You hear the sound of a modem trying to start up.>"
    };

    private string[] reminderDialogue =
    {
        "Have you found the COOLEST THING EVER yet?",
        "Please be sure it is quantifiably the coolest thing you've ever set paw upon."
    };

    protected override void Start()
    {
        base.Start();
        charName = "Parsley";
        gameState = FindObjectOfType<GameState>();
        inventoryManager = FindObjectOfType<InventoryManager>();

    }

    protected override string[] GetDialogue()
    {
        if (gameState.reputation < 2) {
            return initialDialogue;
        }

        if (gameState.reputation >= 3 && gameState.findCD == QuestState.Unavailable)
        {
            gameState.findCD = QuestState.Available;
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
                else { 
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
