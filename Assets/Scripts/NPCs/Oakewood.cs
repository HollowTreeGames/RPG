using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Oakewood : NPC {
    
    private GameState gameState;
    private InventoryManager inventoryManager;
    private System.Random random = new System.Random();

    private string[] cyclingDialogue =
    {
        "Everyone's still mad at you, you know.",
        "You've been known to screw things up in the past, Belfry, but this time just takes the cake.", 
        "*sigh*"
    };
    private IEnumerator<string> cyclingDialogueEnumerator;
    private string[] defaultDialogue =
    {
        "I feel pretty, oh so pretty."
    };

    protected override void Start()
    {
        base.Start();
        charName = "Oakewood";
        gameState = FindObjectOfType<GameState>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        cyclingDialogueEnumerator = CycleDialogue();
    }

    private IEnumerator<string> CycleDialogue()
    {
        while (true) {
            foreach (string sentence in cyclingDialogue)
            {
                yield return sentence;
            }
        }
    }

    protected override string[] GetDialogue()
    {
        cyclingDialogueEnumerator.MoveNext();
        string[] dialogue = { cyclingDialogueEnumerator.Current };
        return dialogue;
    }
}
