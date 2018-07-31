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
    private string[] friendlyDialogue =
    {
        "That was extremely unnecessary.",
        "You know that poor fox has a horrible marijuana addiction.", 
        "... oh, don't look at me like that.", 
        "*sigh* FINE, I'll talk to Parsley so he'll stop giving you the silent treatment."
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
        if ((gameState.reputation > 0) && (gameState.friendshipParsley < 1))
        {
            gameState.reputation += 1;
            gameState.friendshipParsley += 1;
            return friendlyDialogue;
        }

        cyclingDialogueEnumerator.MoveNext();
        string[] dialogue = { cyclingDialogueEnumerator.Current };
        return dialogue;
    }
}
