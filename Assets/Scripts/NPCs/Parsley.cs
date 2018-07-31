using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Parsley : NPC {
    
    private GameState gameState;
    private InventoryManager inventoryManager;

    private string[] initialDialogue =
    {
        "Hmph."
    };
    private string[] friendlyDialogue =
    {
        "I guess you didn't MEAN to burn down the whole town...", 
        "Fine, we can talk again. But don't think I'm ever loaning you another candle!"
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

        return friendlyDialogue;
    }
}
