using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Henry : NPC {
    
    private GameState gameState;
    private InventoryManager inventoryManager;
    private bool hasTalked = false;
    private System.Random random = new System.Random();

    // Initial dialogues
    private string[] initialDialogue =
    {
        "Hi! My name is Henry! I'm the sherriff!",
        "I like sniffing butts!",
        "Nora likes it when I don't wear pants, but I don't know why!"
    };
    private string[] errorDialogue =
    {
        "ERR-OR, I AM A ROBOT, SOMETHING HAS GONE WRONG, BEEP BOOP"
    };

    // Find The Dank Herb, natch
    private string[] questHerbDialogue =
    {
        "Nora has been really stressed by her work lately. You know what she needs?",
        "That's right! A little bit of kibbles and hits!",
        "Would you please find me some nutritious nug to help Nora take the edge off?"
    };
    private string[] questHerbReminderDialogue =
    {
        "Have you found some leafs of the devil's lettuce?"
    };
    private string[] itemHerbDialogue =
    {
        "Wow, thank you! Nora will be so happy to smoke this straight killer kush!"
    };

    // Find the book, y'all
    private string[] questBookDialogue =
    {
        "So I wanted to get Nora a special book...",
        "You didn't hear this from me, but it's a book about...",
        "WEED.",
        "Can you find one for me? She's hopeless at blazing 420 365 blaze it."
    };
    private string[] questBookReminderDialogue =
    {
        "Have you found that book yet? Keep it on the down-low. I'm a sheriff, you know."
    };
    private string[] itemBookDialogue =
    {
        "Thank goodness! I was worried Nora would choke on a blunt without this."
    };

    // You've done all the quests! Thanks.
    private string[] thanksDialogue =
    {
        "Thanks again for the dank dire doobies and this mad awesome book!",
        "Nora asked me to stop saying dank, but it's too much fun!",
        "Dank dank dank dank dank dank dank dank dank dank dank " +
            "dank dank dank dank dank dank dank dank dank dank dank " +
            "dank dank dank dank dank dank dank dank dank dank dank " +
            "dank dank dank dank dank dank dank dank dank dank dank " +
            "dank dank dank dank dank dank dank dank dank dank dank!"
    };

    protected override void Start()
    {
        base.Start();
        charName = "Henry";
        gameState = FindObjectOfType<GameState>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    protected override string[] GetDialogue()
    {
        if (!hasTalked)
        {
            hasTalked = true;
            int randomNumber = random.Next(0, 2);
            Debug.Log(randomNumber);
            if (randomNumber == 1) {
                gameState.findTheDankHerb = QuestState.Available;
            } else
            {
                gameState.findHerbBook = QuestState.Available;
            }
            return initialDialogue;
        }

        if ((gameState.findTheDankHerb == QuestState.Done) && (gameState.findHerbBook == QuestState.Unavailable))
        {
            gameState.findHerbBook = QuestState.Available;
        }

        if ((gameState.findHerbBook == QuestState.Done) && (gameState.findTheDankHerb == QuestState.Unavailable))
        {
            gameState.findTheDankHerb = QuestState.Available;
        }

        if ((gameState.findHerbBook == QuestState.Done) && (gameState.findTheDankHerb == QuestState.Done))
        {
            return thanksDialogue;
        }

        // Find dat dank herb
        switch (gameState.findTheDankHerb)
        {
            case QuestState.Available:
                gameState.findTheDankHerb = QuestState.InProgress;
                return questHerbDialogue;
            case QuestState.InProgress:
                if (inventoryManager.GetInventory() == "Dank Herb")
                {
                    gameState.reputation += 1;
                    gameState.friendshipHenry += 1;
                    inventoryManager.ClearInventory();
                    gameState.findTheDankHerb = QuestState.Done;
                    return itemHerbDialogue;
                }
                else
                    return questHerbReminderDialogue;
            default:
                break;
        }

        // Get you some herb book, son
        switch (gameState.findHerbBook)
        {
            case QuestState.Available:
                gameState.findHerbBook = QuestState.InProgress;
                return questBookDialogue;
            case QuestState.InProgress:
                if (inventoryManager.GetInventory() == "Herb Book")
                {
                    gameState.reputation += 1;
                    gameState.friendshipHenry += 1;
                    inventoryManager.ClearInventory();
                    gameState.findHerbBook = QuestState.Done;
                    return itemBookDialogue;
                }
                else
                    return questBookReminderDialogue;
            default:
                break;
        }

        return errorDialogue;
    }
}
