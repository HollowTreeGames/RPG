using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Henry : NPC {
    
    private GameState gameState;
    private InventoryManager inventoryManager;

    private string[] initialDialogue =
    {
        "Hi! My name is Henry! I'm the sherriff!",
        "I like sniffing butts!",
        "Nora likes it when I don't wear pants, but I don't know why!"
    };
    private string[] repeatingDialogue =
    {
        "What can I do you for?"
    };
    private string[] itemDialogue =
    {
        "Wow, thank you! Nora will be so happy to smoke this straight killer kush!"
    };
    private string[] thanksDialogue =
    {
        "Thanks again for the dank dire doobies!",
        "Nora asked me to stop saying dank, but it's too much fun!",
        "Dank dank dank dank dank dank dank dank dank dank dank " +
            "dank dank dank dank dank dank dank dank dank dank dank " +
            "dank dank dank dank dank dank dank dank dank dank dank " +
            "dank dank dank dank dank dank dank dank dank dank dank " +
            "dank dank dank dank dank dank dank dank dank dank dank!"
    };
    private string[] questDialogue =
    {
        "Nora has been really stressed by her work lately. You know what she needs?", 
        "That's right! A little bit of kibbles and hits!", 
        "Would you please find me some nutritious nug to help Nora take the edge off?"
    };
    private string[] questReminderDialogue =
    {
        "Please hurry back with a few leafs of the devil's lettuce!"
    };

    private bool gotItem = false;

    protected override void Start()
    {
        base.Start();
        charName = "Henry";
        gameState = FindObjectOfType<GameState>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    private bool interactedWith = false;

    protected override string[] GetDialogue()
    {
        switch(gameState.findTheDankHerb)
        {
            case QuestState.Unavailable:
                gameState.findTheDankHerb = QuestState.Available;
                return initialDialogue;
            case QuestState.Available:
                gameState.findTheDankHerb = QuestState.InProgress;
                return questDialogue;
            case QuestState.InProgress:
                if (inventoryManager.GetInventory() == "Dank Herb")
                {
                    gameState.reputation += 1;
                    gameState.friendshipHenry += 1;
                    inventoryManager.ClearInventory();
                    gameState.findTheDankHerb = QuestState.Done;
                    return itemDialogue;
                }
                else
                    return questReminderDialogue;
            case QuestState.Done:
                return thanksDialogue;
            default:
                break;
        }

        return repeatingDialogue;
    }
}
